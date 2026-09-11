using System.Linq.Expressions;

using Anch.Core;
using Anch.GenericQueryable;
using Anch.GenericQueryable.Fetching;

using Framework.Core;
using Framework.Database.Domain;
using Framework.Database.EntityFramework.EnversAudit;

namespace Framework.Database.EntityFramework;

public class EfDal<TDomainObject, TIdent>(
    IAsyncDal<TDomainObject, TIdent> asyncDal,
    IEfAuditReader auditReader,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null) : IDAL<TDomainObject, TIdent>
    where TDomainObject : class
    where TIdent : notnull
{
    public TDomainObject GetById(TIdent id, LockRole lockRole)
    {
        var domainObject = asyncDal.Load(id);

        if (lockRole != LockRole.None)
        {
            this.Lock(domainObject, lockRole);
        }

        return domainObject;
    }

    public void Lock(TDomainObject domainObject, LockRole lockRole) => defaultCancellationTokenSource.RunSync(ct => asyncDal.LockAsync(domainObject, lockRole, ct));

    public void Refresh(TDomainObject domainObject) => defaultCancellationTokenSource.RunSync(ct => asyncDal.RefreshAsync(domainObject, ct));

    public virtual void Save(TDomainObject domainObject) => defaultCancellationTokenSource.RunSync(ct => asyncDal.SaveAsync(domainObject, ct));

    public virtual void Insert(TDomainObject domainObject, TIdent id) => defaultCancellationTokenSource.RunSync(ct => asyncDal.InsertAsync(domainObject, id, ct));

    public virtual void Remove(TDomainObject domainObject) => defaultCancellationTokenSource.RunSync(ct => asyncDal.RemoveAsync(domainObject, ct));

    public IQueryable<TDomainObject> GetQueryable(LockRole lockRole, FetchRule<TDomainObject>? fetchRule = null)
    {
        var queryable = asyncDal.GetQueryable();

        var withFetchQueryable = fetchRule is null ? queryable : queryable.WithFetch(fetchRule);

        if (lockRole != LockRole.None)
        {
            throw new NotSupportedException("EF backend doesn't support queryable-level locking. Use GetById(id, lockRole) instead.");
        }

        return withFetchQueryable;
    }

    public TDomainObject Load(TIdent id) => asyncDal.Load(id);

    public TDomainObject GetObjectByRevision(TIdent id, long revision) => auditReader.Find<TDomainObject>(id, revision);

    public IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> idCollection, long revisionNumber) =>
        auditReader.FindObjects<TDomainObject>(idCollection.Cast<object>(), revisionNumber);

    public IEnumerable<long> GetRevisions(TIdent id) => auditReader.GetRevisions(typeof(TDomainObject), id);

    public IReadOnlyList<Tuple<T, long>> GetDomainObjectRevisions<T>(TIdent id, int takeCount)
        where T : class =>
        auditReader.GetDomainObjectRevisions<T>(id, takeCount);

    public IEnumerable<long> GetRevisions(TIdent id, long maxRevision) => auditReader.GetRevisions(typeof(TDomainObject), id, maxRevision);

    public long? GetPreviousRevision(TIdent id, long maxRevision) => auditReader.GetPreviousRevision(typeof(TDomainObject), id, maxRevision);

    public long GetCurrentRevision() => auditReader.GetCurrentRevision();

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(
        TIdent id,
        string propertyName,
        Period? period = null)
    {
        var result = new DomainObjectPropertyRevisions<TIdent, TProperty>(id, propertyName);

        foreach (var revision in auditReader.GetPropertyRevisions<TProperty>(typeof(TDomainObject), id, propertyName, period?.StartDate, period?.EndDate))
        {
            _ = new PropertyRevision<TIdent, TProperty>(
                result,
                revision.Value,
                ToAuditRevisionType(revision.Revision.RevisionType),
                revision.Revision.Author,
                revision.Revision.Date,
                revision.Revision.RevisionNumber);
        }

        return result;
    }

    public IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUntypedPropertyRevisions(
        TIdent id,
        string propertyName,
        Period? period = null)
    {
        var propertyInfo = typeof(TDomainObject).GetProperties()
                                                .First(z => string.Equals(z.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

        var genericMethodDefinition =
            ((Func<TIdent, string, Period?, DomainObjectPropertyRevisions<TIdent, object>>)this.GetPropertyRevisions<object>)
            .Method
            .GetGenericMethodDefinition();

        var method = genericMethodDefinition.MakeGenericMethod(propertyInfo.PropertyType);

        return (IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase>)method.Invoke(this, [id, propertyName, period])!;
    }

    public DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyRevisions<TProperty>(
        TIdent id,
        Expression<Func<TDomainObject, TProperty>> propertyExpression,
        Period? period = null) =>
        this.GetPropertyRevisions<TProperty>(id, GetMemberName(propertyExpression.Body), period);

    public DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null)
    {
        var result = new DomainObjectRevision<TIdent>(identity);

        foreach (var revision in auditReader.GetObjectRevisions(typeof(TDomainObject), identity, period?.StartDate, period?.EndDate))
        {
            _ = new DomainObjectRevisionInfo<TIdent>(result, ToAuditRevisionType(revision.RevisionType), revision.Author, revision.Date, revision.RevisionNumber);
        }

        return result;
    }

    public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomainObject, bool>> query) =>
        auditReader.GetIdentiesWithHistory<TDomainObject, TIdent>(query);

    private static string GetMemberName(Expression expression) => expression switch
    {
        MemberExpression member => member.Member.Name,
        UnaryExpression unary => GetMemberName(unary.Operand),
        _ => throw new NotSupportedException($"Expression \"{expression}\" is not a simple property access."),
    };

    private static Framework.Database.Domain.AuditRevisionType ToAuditRevisionType(EnversAudit.AuditRevisionType revisionType) => revisionType switch
    {
        EnversAudit.AuditRevisionType.Added => Framework.Database.Domain.AuditRevisionType.Added,
        EnversAudit.AuditRevisionType.Modified => Framework.Database.Domain.AuditRevisionType.Modified,
        EnversAudit.AuditRevisionType.Deleted => Framework.Database.Domain.AuditRevisionType.Deleted,
        _ => throw new ArgumentOutOfRangeException(nameof(revisionType), revisionType, null),
    };
}
