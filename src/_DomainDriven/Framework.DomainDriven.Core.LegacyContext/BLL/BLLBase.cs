using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.Lock;
using Framework.Exceptions;
using Framework.OData;
using Framework.Persistent;

using GenericQueryable;
using GenericQueryable.Fetching;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL;

public abstract class BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent> :
    OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject>,
    IBLLBase<TBLLContext, TDomainObject>
    where TBLLContext : class, IBLLBaseContext
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TIdent: notnull
{
    private readonly IDAL<TDomainObject, TIdent> dal;

    protected BLLBase(TBLLContext context)
        : base(context)
    {
        this.dal = this.Context.ServiceProvider.GetRequiredService<IDAL<TDomainObject, TIdent>>();
    }

    #region Private.Method

    protected virtual IQueryable<TDomainObject> ProcessSecurity(IQueryable<TDomainObject> queryable)
    {
        return queryable;
    }

    #endregion

    private void InternalSave(TDomainObject domainObject, TIdent id = default(TIdent))
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (id.IsDefault())
        {
            this.dal.Save(domainObject);
        }
        else
        {
            this.dal.Insert(domainObject, id);
        }

        base.Save(domainObject);
    }

    public override void Save(TDomainObject domainObject)
    {
        this.InternalSave(domainObject);
    }

    public virtual void Insert(TDomainObject domainObject, TIdent id)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        var isSimpleSave = !domainObject.Id.IsDefault()
                           && this.GetUnsecureQueryable().Select(z => z.Id).Any(d => d.Equals(domainObject.Id));

        this.InternalSave(domainObject, isSimpleSave ? default! : id);
    }

    public override void Remove(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.dal.Remove(domainObject);
        base.Remove(domainObject);
    }

    public abstract SelectOperationResult<TProjection> GetObjectsByOData<TProjection>(
        SelectOperation<TProjection> selectOperation,
        Expression<Func<TDomainObject, TProjection>> projectionSelector);

    protected void SaveWithoutCascade(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        var actions = CascadeActionHelper.Actions.ToArray(f => f(domainObject));

        actions.Foreach(pair => pair.Item1());

        try
        {
            this.InternalSave(domainObject);
        }
        finally
        {
            actions.Foreach(pair => pair.Item2());
        }
    }

    protected void InsertWithoutCascade(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.InsertWithoutCascade(domainObject, domainObject.Id);
    }

    protected void InsertWithoutCascade(TDomainObject domainObject, TIdent id)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        var actions = CascadeActionHelper.Actions.ToArray(f => f(domainObject));

        actions.Foreach(pair => pair.Item1());

        try
        {
            this.InternalSave(domainObject, id);
        }
        finally
        {
            actions.Foreach(pair => pair.Item2());
        }
    }

    /// <summary>
    /// Return the persistent instance of the given entity class with the given identifier,
    /// assuming that the instance exists.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TDomainObject Load(TIdent id) => this.dal.Load(id);

    public List<TDomainObject> GetListBy(
        Expression<Func<TDomainObject, bool>> filter,
        FetchRule<TDomainObject>? fetchRule = null,
        LockRole lockRole = LockRole.None)
    {
        return ((IEnumerable<TDomainObject>)this.GetSecureQueryable(fetchRule, lockRole).Where(filter)).Distinct().ToList();
    }

    public List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetListBy(filter, buildFetchRule.ToFetchRule());

    public List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None) =>
        this.GetListBy(filter.ToFilterExpression(), fetchRule, lockRole);

    public List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetListBy(filter, buildFetchRule.ToFetchRule());

    public abstract SelectOperationResult<TDomainObject> GetObjectsByOData(
        SelectOperation selectOperation,
        FetchRule<TDomainObject>? fetchRule = null);

    public abstract SelectOperationResult<TDomainObject> GetObjectsByOData(
        SelectOperation<TDomainObject> selectOperation,
        FetchRule<TDomainObject>? fetchRule = null);

    public SelectOperationResult<TDomainObject> GetObjectsByOData(
        SelectOperation<TDomainObject> selectOperation,
        Expression<Func<TDomainObject, bool>> filter,
        FetchRule<TDomainObject>? fetchRule = null)
    {
        var selectOperationWithFilter = selectOperation.AddFilter(filter);

        return this.GetObjectsByOData(selectOperationWithFilter, fetchRule);
    }

    public SelectOperationResult<TDomainObject> GetObjectsByOData(
        SelectOperation<TDomainObject> selectOperation,
        IDomainObjectFilterModel<TDomainObject> filter,
        FetchRule<TDomainObject>? fetchRule = null) =>
        this.GetObjectsByOData(selectOperation, filter.ToFilterExpression(), fetchRule);

    public List<TDomainObject> GetFullList(FetchRule<TDomainObject>? fetchRule = null)
    {
        return this.GetSecureQueryable(fetchRule).ToList();
    }

    public List<TDomainObject> GetFullList(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetFullList(buildFetchRule.ToFetchRule());

    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <param name="lockRole">Тип блокировки</param>
    /// <param name="fetchRule">Подгружаемые свойства</param>
    /// <returns></returns>
    public IQueryable<TDomainObject> GetUnsecureQueryable(FetchRule<TDomainObject>? fetchRule, LockRole lockRole = LockRole.None)
    {
        return this.dal.GetQueryable(lockRole, fetchRule);
    }

    public IQueryable<TDomainObject> GetUnsecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetUnsecureQueryable(buildFetchRule.ToFetchRule());

    /// <summary>
    /// Получение IQueryable с учётом безопасности
    /// </summary>
    /// <param name="lockRole">Тип блокировки</param>
    /// <param name="fetchRule">Подгружаемые свойства</param>
    /// <returns></returns>
    public IQueryable<TDomainObject> GetSecureQueryable(
        FetchRule<TDomainObject>? fetchRule = null,
        LockRole lockRole = LockRole.None)
    {
        return this.ProcessSecurity(this.GetUnsecureQueryable(fetchRule, lockRole));
    }

    public IQueryable<TDomainObject> GetSecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetSecureQueryable(buildFetchRule.ToFetchRule());

    protected IQueryable<TDomainObject> GetSecureQueryable(
        IQueryableProcessor<TDomainObject> baseProcessor,
        FetchRule<TDomainObject>? fetchRule = null)
    {
        if (baseProcessor == null) throw new ArgumentNullException(nameof(baseProcessor));

        return this.GetSecureQueryable(fetchRule).Pipe(q => baseProcessor.Process(q));
    }

    /// <summary>
    /// Выполняет поиск доменного объекта по условию.
    /// </summary>
    /// <param name="filter">Выражение, определяющее условие поиска.</param>
    /// <param name="getNotFoundException">Делегат, возвращающий исключение, которое необходимо бросить, если объект не найден.</param>
    /// <param name="fetchRule">Пути, определяющие выгрузку графа.</param>
    /// <returns>Объект, найденный по условию.</returns>
    /// <exception cref="ArgumentNullException">Аргумент
    /// <paramref name="filter"/>
    /// or
    /// <paramref name="getNotFoundException"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
    public TDomainObject GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        Func<Exception> getNotFoundException,
        FetchRule<TDomainObject>? fetchRule = null)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));
        if (getNotFoundException == null) throw new ArgumentNullException(nameof(getNotFoundException));

        return this.GetObjectBy(filter, false, fetchRule).FromMaybe(getNotFoundException);
    }

    /// <summary>
    /// Выполняет поиск доменного объекта по условию.
    /// </summary>
    /// <param name="lockRole">Экземпляр <see cref="LockRole"/>, определяющий запрашиваемую блокировку.</param>
    /// <param name="filter">Выражение, определяющее условие поиска.</param>
    /// <param name="fetchRule">Пути, определяющие выгрузку графа.</param>
    /// <param name="throwOnNotFound">
    ///   Когда передано <c>true</c> будет выброшено <see cref="BusinessLogicException"/>.
    ///   если объект по условию не найден.
    /// </param>
    /// <returns>Экземпляр найденного объекта или null, если объект не найден.</returns>
    /// <exception cref="BusinessLogicException">Если параметр throwOnNotFound задан как true и объект не найден.</exception>
    /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
    [Obsolete("Эта перегрузка метода будет удалёна в следующих версиях. Используйте вместо неё другие перегрузки.")]
    public TDomainObject? GetObjectBy(
        LockRole lockRole,
        Expression<Func<TDomainObject, bool>> filter,
        FetchRule<TDomainObject>? fetchRule = null,
        bool throwOnNotFound = false)
    {
        return this.GetObjectBy(lockRole, filter, throwOnNotFound, fetchRule);
    }

    /// <inheritdoc />
    public TDomainObject? GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        bool throwOnNotFound = false,
        FetchRule<TDomainObject>? fetchRule = null)
    {
        return this.GetObjectBy(LockRole.None, filter, throwOnNotFound, fetchRule);
    }

    public TDomainObject? GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        bool throwOnNotFound,
        Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetObjectBy(filter, throwOnNotFound, buildFetchRule.ToFetchRule());

    /// <summary>
    /// Выполняет поиск доменного объекта по условию.
    /// </summary>
    /// <param name="lockRole">Экземпляр <see cref="LockRole"/>, определяющий запрашиваемую блокировку.</param>
    /// <param name="filter">Выражение, определяющее условие поиска.</param>
    /// <param name="throwOnNotFound">
    ///   Когда передано <c>true</c> будет выброшено <see cref="BusinessLogicException"/>.
    ///   если объект по условию не найден.
    /// </param>
    /// <param name="fetchRule">Пути, определяющие выгрузку графа.</param>
    /// <returns>Экземпляр найденного объекта или null, если объект не найден.</returns>
    /// <exception cref="BusinessLogicException">Если параметр throwOnNotFound задан как true и объект не найден.</exception>
    /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
    public TDomainObject? GetObjectBy(
        LockRole lockRole,
        Expression<Func<TDomainObject, bool>> filter,
        bool throwOnNotFound = false,
        FetchRule<TDomainObject>? fetchRule = null)
    {
        var result = this.GetListBy(filter, fetchRule, lockRole).SingleOrDefault();

        if (null == result && throwOnNotFound)
        {
            throw new BusinessLogicException($"Object {typeof(TDomainObject).Name} not found by: {filter}.");
        }

        return result;
    }

    public virtual void Lock(TDomainObject domainObject, LockRole lockRole)
    {
        this.dal.Lock(domainObject, lockRole);
    }

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    public virtual void Refresh(TDomainObject domainObject)
    {
        this.dal.Refresh(domainObject);
    }

    #region Revision

    public virtual TDomainObject GetObjectByRevision(TIdent id, long revision)
    {
        return this.dal.GetObjectByRevision(id, revision);
    }

    public virtual long? GetPreviousRevision(TIdent id, long maxRevision)
    {
        return this.dal.GetPreviousRevision(id, maxRevision);
    }

    public virtual TDomainObject GetObjectsByPrevRevision(TIdent id)
    {
        var objectsRevisions = this.GetObjectRevisions(id).RevisionInfos.OrderByDescending(info => info.Date).ToList();

        var revision = objectsRevisions
                       .First(() => new ArgumentException($"Object with id:{id} are not found. (Type:{typeof(TDomainObject).Name})"))
                       .RevisionNumber;

        return this.GetObjectByRevision(id, revision);
    }

    public virtual IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> idCollection, long revision)
    {
        return this.dal.GetObjectsByRevision(idCollection, revision);
    }

    public virtual DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null)
    {
        return this.dal.GetObjectRevisions(identity, period);
    }

    public virtual long? GetPreviusVersion(TIdent id, long maxRevisionNumber)
    {
        return this.dal.GetPreviousRevision(id, maxRevisionNumber);
    }

    public virtual IEnumerable<long> GetRevisions(TIdent id)
    {
        return this.dal.GetRevisions(id);
    }

    public virtual IReadOnlyList<Tuple<TDomainObject, long>> GetDomainObjectRevisions(TIdent id, int takeCount)
    {
        return this.dal.GetDomainObjectRevisions<TDomainObject>(id, takeCount);
    }

    public virtual DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(
        TIdent id,
        Expression<Func<TDomainObject, TProperty>> propertyExpression,
        Period? period = null)
    {
        return this.dal.GetPropertyRevisions(id, propertyExpression, period);
    }

    public virtual DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(
        TIdent id,
        string propertyName,
        Period? period = null)
    {
        return this.dal.GetPropertyRevisions<TProperty>(id, propertyName, period);
    }

    public virtual IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUnTypedPropertyChanges(
        TIdent id,
        string propertyName,
        Period? period = null)
    {
        return this.dal.GetUntypedPropertyRevisions(id, propertyName, period);
    }

    public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomainObject, bool>> expression)
    {
        return this.dal.GetIdentiesWithHistory(expression);
    }

    /// <summary>
    /// Get current revision. Actual info only after flush
    /// </summary>
    /// <returns></returns>
    public long GetCurrentRevision()
    {
        return this.dal.GetCurrentRevision();
    }

    #endregion

    private static class CascadeActionHelper
    {
        public static readonly ReadOnlyCollection<Func<TDomainObject, Tuple<Action, Action>>> Actions = typeof(TDomainObject)
            .GetDetailTypes()
            .Select(GetActions)
            .ToReadOnlyCollection();

        private static Func<TDomainObject, Tuple<Action, Action>> GetActions(Type detaiType)
        {
            return new Func<Func<IMaster<TDomainObject>, Tuple<Action, Action>>>(GetActions<IMaster<TDomainObject>, TDomainObject>)
                   .CreateGenericMethod(typeof(TDomainObject), detaiType)
                   .Invoke<Func<TDomainObject, Tuple<Action, Action>>>(null);
        }

        private static Func<TMaster, Tuple<Action, Action>> GetActions<TMaster, TDetail>()
            where TMaster : IMaster<TDetail>
        {
            return domainObject =>
                   {
                       var details = domainObject.Details.ToArray();

                       return new Tuple<Action, Action>(() => domainObject.Details.Clear(), () => domainObject.Details.AddRange(details));
                   };
        }
    }

    public IQueryable<TDomainObject> GetUnsecureQueryable() => this.GetUnsecureQueryable(null);
}
