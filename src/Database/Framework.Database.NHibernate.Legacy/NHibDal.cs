using System.Linq.Expressions;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;

using Framework.Core;
using Framework.Database.Domain;
using Framework.Database.NHibernate.DAL.Revisions;
using Framework.Database.NHibernate.Envers;
using Framework.Database.NHibernate.Envers.LinqVisitors;
using Framework.Database.NHibernate.Sessions;

using GenericQueryable;
using GenericQueryable.Fetching;

using NHibernate;
using NHibernate.Envers.Query;
using NHibernate.Envers.Query.Criteria;
using NHibernate.Linq;

namespace Framework.Database.NHibernate;

public class NHibDal<TDomainObject, TIdent>(INHibSession session, IAsyncDal<TDomainObject, TIdent> asyncDal) : IDAL<TDomainObject, TIdent>
    where TDomainObject : class
{
    private static readonly LambdaCompileCache LambdaCompileCache = new(LambdaCompileMode.None);

    private ISession NativeSession => session.NativeSession;

    public TDomainObject GetById(TIdent id, LockRole lockRole) => this.NativeSession.Get<TDomainObject>(id, lockRole.ToLockMode());

    public void Lock(TDomainObject domainObject, LockRole lockRole) => asyncDal.LockAsync(domainObject, lockRole).GetAwaiter().GetResult();

    public void Refresh(TDomainObject domainObject) => this.NativeSession.Refresh(domainObject);

    public virtual void Save(TDomainObject domainObject) => asyncDal.SaveAsync(domainObject).GetAwaiter().GetResult();

    public virtual void Insert(TDomainObject domainObject, TIdent id) => asyncDal.InsertAsync(domainObject, id).GetAwaiter().GetResult();

    public virtual void Remove(TDomainObject domainObject) => asyncDal.RemoveAsync(domainObject).GetAwaiter().GetResult();

    public IQueryable<TDomainObject> GetQueryable(LockRole lockRole, FetchRule<TDomainObject>? fetchRule = null)
    {
        var queryable = asyncDal.GetQueryable();

        var withFetchQueryable = fetchRule == null ? queryable : queryable.WithFetch(fetchRule);

        if (lockRole == LockRole.None)
        {
            return withFetchQueryable;
        }

        return withFetchQueryable.WithLock(lockRole.ToLockMode());
    }

    public TDomainObject GetObjectByRevision(TIdent id, long revision) => this.GetAuditReader().Find<TDomainObject>(id, revision);

    public IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> idCollection, long revisionNumber) =>
        this.GetAuditReader().FindObjects<TDomainObject>(idCollection.Cast<object>(), revisionNumber);

    public IEnumerable<long> GetRevisions(TIdent id) => this.GetAuditReader().GetRevisions(typeof(TDomainObject), id).ToList();

    public IReadOnlyList<Tuple<T, long>> GetDomainObjectRevisions<T>(TIdent id, int takeCount)
        where T : class =>
        this.GetAuditReader().GetDomainObjectRevisions<T>(id, takeCount);

    public IEnumerable<long> GetRevisions(TIdent id, long maxRevision) =>
        this.GetAuditReader().GetRevisions(typeof(TDomainObject), id, maxRevision).ToList();

    public long? GetPreviousRevision(TIdent id, long maxRevision) =>
        this.GetAuditReader().GetPreviousRevision(typeof(TDomainObject), id, maxRevision);

    public long GetCurrentRevision() => this.GetAuditReader().GetCurrentRevision<AuditRevisionEntity>(false).Id;

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(
        TIdent id,
        string propertyName,
        Period? period = null)
    {
        var propertyInfo = typeof(TDomainObject).GetProperties()
                                                .FirstOrDefault(
                                                    z => string.Equals(
                                                        z.Name,
                                                        propertyName,
                                                        StringComparison.InvariantCultureIgnoreCase));
        if (null == propertyInfo)
        {
            throw new InvalidOperationException($"{typeof(TDomainObject).Name} {propertyName}");
        }

        var domainObjectParameter = Expression.Parameter(typeof(TDomainObject), "z");
        var property = Expression.Property(domainObjectParameter, propertyName);

        var propertyExpression = Expression.Lambda<Func<TDomainObject, TProperty>>(property, domainObjectParameter);

        return this.GetPropertyRevisions(id, propertyExpression, period);
    }

    public IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUntypedPropertyRevisions(
        TIdent id,
        string propertyName,
        Period? period = null)
    {
        var propertyInfo = typeof(TDomainObject).GetProperties()
                                                .First(
                                                    z => string.Equals(
                                                        z.Name,
                                                        propertyName,
                                                        StringComparison.InvariantCultureIgnoreCase));

        var genericMethodDefinition =
            ((Func<TIdent, string, Period?, DomainObjectPropertyRevisions<TIdent, object>>)this.GetPropertyRevisions<object>)
            .Method
            .GetGenericMethodDefinition();
        var method = genericMethodDefinition.MakeGenericMethod(propertyInfo.PropertyType);

        var result = method.InvokeWithExceptionProcessed(this, id, propertyName, period);

        return (IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase>)result;
    }

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(
        TIdent id,
        Expression<Func<TDomainObject, TProperty>> propertyExpression,
        Period? period = null)
    {
        var propertyPath = propertyExpression.ToPath();

        if ((typeof(TProperty).IsPrimitive || typeof(TProperty) == typeof(decimal) || typeof(TProperty) == typeof(string))
            && !propertyPath.Contains("."))
        {
            return this.GetPrimitivePropertiesRevision(id, propertyExpression, period);
        }

        var queryCreator = this.GetAuditReader().CreatePatchedQuery();

        var rootPropertyName = new string(propertyPath.TakeWhile(z => z != '.').ToArray());

        var changeOrFirstCriterion = AuditEntity.Or(
            AuditEntity.Property(rootPropertyName).HasChanged(),
            AuditEntity.RevisionType().Eq(AuditRevisionType.Added));

        var query = queryCreator
                    .ForProjectingRevisionsOfEntity<TDomainObject>(false, true)
                    .Add(AuditEntity.Id().Eq(id))
                    .Add(changeOrFirstCriterion);

        query = this.TryInjectPeriodQuery(query, period);

        var queryResults = query.GetResultList();

        var result = new DomainObjectPropertyRevisions<TIdent, TProperty>(id, propertyPath);

        var getPropertyFunc = LambdaCompileCache.GetFunc(propertyExpression);

        foreach (var queryResult in queryResults.Cast<object[]>())
        {
            var domainObject = (TDomainObject)queryResult[0];
            var auditRevisionEntiry = (AuditRevisionEntity)queryResult[1];
            new PropertyRevision<TIdent, TProperty>(
                result,
                getPropertyFunc(domainObject),
                AuditRevisionType.Modified,
                auditRevisionEntiry.Author,
                auditRevisionEntiry.RevisionDate,
                auditRevisionEntiry.Id);
        }

        return result;
    }

    public DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null)
    {
        var auditReaderPatched = this.GetAuditReader();

        var auditQuery = auditReaderPatched.CreatePatchedQuery()
                                           .ForHistoryOf<TDomainObject, AuditRevisionEntity, TIdent>(true)
                                           .Add(AuditEntity.Id().Eq(identity));

        auditQuery = this.TryInjectPeriodQuery(auditQuery, period);

        var audited = auditQuery.Results().ToList();

        var result = new DomainObjectRevision<TIdent>(identity);

        audited.Foreach(
            z =>
            {
                new DomainObjectRevisionInfo<TIdent>(
                    result,
                    z.Operation.ToAuditRevisionType(),
                    z.RevisionEntity.Author,
                    z.RevisionEntity.RevisionDate,
                    z.RevisionEntity.Id);
            });

        return result;
    }

    public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomainObject, bool>> query) =>
        this.GetAuditReader().GetIdentsBy<TDomainObject, TIdent>(query.ToCriterion());

    public TDomainObject GetById(TIdent id) => this.NativeSession.Get<TDomainObject>(id);

    public TDomainObject Load(TIdent id) => this.NativeSession.Load<TDomainObject>(id);

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPrimitivePropertiesRevision<TProperty>(
        TIdent id,
        Expression<Func<TDomainObject, TProperty>> propertyExpression,
        Period? period = null)
    {
        var propertyName = propertyExpression.ToPath();

        var queryCreator = this.GetAuditReader().CreateQuery();

        var changeOrFirstCriterion = AuditEntity.Or(
            AuditEntity.Property(propertyName).HasChanged(),
            AuditEntity.RevisionType().Eq(AuditRevisionType.Added));

        var query = queryCreator
                    .ForRevisionsOfEntity(typeof(TDomainObject), false, true)
                    .Add(AuditEntity.Id().Eq(id))
                    .Add(changeOrFirstCriterion)
                    .AddProjection(AuditEntity.Property(propertyName))
                    .AddProjection(AuditEntity.RevisionType())
                    .AddProjection(AuditEntity.RevisionNumber());

        query = this.TryInjectPeriodQuery(query, period);

        var isAuditedType = typeof(IAuditObject).IsAssignableFrom(typeof(TDomainObject));

        if (isAuditedType)
        {
            query = query.AddProjection(AuditEntity.Property(AuditObjectHelper.ModifyDatePropertyName))
                         .AddProjection(AuditEntity.Property(AuditObjectHelper.ModifyByPropertyName));
        }

        var result = new DomainObjectPropertyRevisions<TIdent, TProperty>(id, propertyName);

        foreach (var resultItem in query.GetResultList().Cast<object[]>())
        {
            var propertyValue = (TProperty)Convert.ChangeType(resultItem[0], typeof(TProperty));

            var revisionType = (AuditRevisionType)((int)resultItem[1]);
            var author = isAuditedType ? resultItem[4].ToString() : string.Empty;
            var revisionNumber = (long)resultItem[2];
            var date = isAuditedType ? DateTime.Parse(resultItem[3].ToString()) : new DateTime(1753, 01, 01);

            new PropertyRevision<TIdent, TProperty>(result, propertyValue, revisionType, author, date, revisionNumber);
        }

        return result;
    }

    private IEntityAuditQuery<T> TryInjectPeriodQuery<T>(IEntityAuditQuery<T> query, Period? period = null)
        where T : class =>
        this.GetPeriodAuditCriterions(period).Aggregate(query, (prevQuery, criterion) => prevQuery.Add(criterion));

    private IAuditQuery TryInjectPeriodQuery(IAuditQuery query, Period? period = null) => this.GetPeriodAuditCriterions(period)
                                                                                              .Aggregate(query, (prevQuery, criterion) => prevQuery.Add(criterion));

    private IEnumerable<IAuditCriterion> GetPeriodAuditCriterions(Period? period = null)
    {
        if (period.HasValue)
        {
            yield return AuditEntity.RevisionProperty("RevisionDate").Ge(period.Value.StartDate);
            yield return AuditEntity.RevisionProperty("RevisionDate").Le(period.Value.EndDate);
        }
    }

    private IAuditReaderPatched GetAuditReader() => session.AuditReader;

    private void CheckWrite()
    {
        if (session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}


