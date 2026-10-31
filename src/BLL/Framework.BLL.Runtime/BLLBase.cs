using System.Linq.Expressions;

using Anch.Core;
using Anch.GenericQueryable;
using Anch.GenericQueryable.Fetching;
using Anch.IdentitySource;
using Anch.OData.Domain;

using Framework.Application;
using Framework.Application.Domain;
using Framework.BLL.Domain.Models;
using Framework.Core;
using Framework.Database;
using Framework.Database.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.BLL;

public abstract class BLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TIdent>(TBLLContext context) :
    OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject>(context),
    IBLLBase<TBLLContext, TDomainObject>
    where TBLLContext : class, IBLLBaseContext
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
    where TIdent : notnull
{
    private IDAL<TDomainObject, TIdent> Dal => field ??= this.Context.ServiceProvider.GetRequiredService<IDAL<TDomainObject, TIdent>>();

    protected IIdentityInfo<TDomainObject, TIdent> IdentityInfo =>
        field ??= this.Context.ServiceProvider.GetRequiredService<IIdentityInfo<TDomainObject, TIdent>>();

    #region Private.Method

    protected virtual IQueryable<TDomainObject> ProcessSecurity(IQueryable<TDomainObject> queryable) => queryable;

    #endregion


    public override void Save(TDomainObject domainObject)
    {
        this.Dal.Save(domainObject);

        base.Save(domainObject);
    }

    public virtual void Insert(TDomainObject domainObject, TIdent id)
    {
        if (domainObject is null) throw new ArgumentNullException(nameof(domainObject));

        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        this.Dal.Insert(domainObject, id);
    }

    public override void Remove(TDomainObject domainObject)
    {
        if (domainObject is null) throw new ArgumentNullException(nameof(domainObject));

        this.Dal.Remove(domainObject);
        base.Remove(domainObject);
    }

    /// <summary>
    /// Return the persistent instance of the given entity class with the given identifier,
    /// assuming that the instance exists.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TDomainObject Load(TIdent id) => this.Dal.Load(id);

    public List<TDomainObject> GetListBy(
        Expression<Func<TDomainObject, bool>> filter,
        FetchRule<TDomainObject>? fetchRule = null,
        LockRole lockRole = LockRole.None) =>
        ((IEnumerable<TDomainObject>)this.GetSecureQueryable(fetchRule, lockRole).Where(filter)).Distinct().ToList();

    public List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule) =>
        this.GetListBy(filter, buildFetchRule.ToFetchRule());

    public List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None) =>
        this.GetListBy(filter.ToFilterExpression(), fetchRule, lockRole);

    public List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule) =>
        this.GetListBy(filter, buildFetchRule.ToFetchRule());

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

    public List<TDomainObject> GetFullList(FetchRule<TDomainObject>? fetchRule = null) => this.GetSecureQueryable(fetchRule).ToList();

    public List<TDomainObject> GetFullList(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetFullList(buildFetchRule.ToFetchRule());

    public IQueryable<TDomainObject> GetUnsecureQueryable() => this.GetUnsecureQueryable(FetchRule<TDomainObject>.Empty);

    public IQueryable<TDomainObject> GetUnsecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetUnsecureQueryable(buildFetchRule.ToFetchRule());

    /// <summary>
    /// Getting IQueryable without security
    /// </summary>
    /// <param name="lockRole">Lock type</param>
    /// <param name="fetchRule">Properties to fetch</param>
    /// <returns></returns>
    public IQueryable<TDomainObject> GetUnsecureQueryable(FetchRule<TDomainObject>? fetchRule, LockRole lockRole = LockRole.None) => this.Dal.GetQueryable(lockRole, fetchRule);

    /// <summary>
    /// Getting IQueryable with security
    /// </summary>
    /// <param name="lockRole">Lock type</param>
    /// <param name="fetchRule">Properties to fetch</param>
    /// <returns></returns>
    public IQueryable<TDomainObject> GetSecureQueryable(FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None) => this.ProcessSecurity(this.GetUnsecureQueryable(fetchRule, lockRole));

    public IQueryable<TDomainObject> GetSecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule) =>
        this.GetSecureQueryable(buildFetchRule.ToFetchRule());

    protected IQueryable<TDomainObject> GetSecureQueryable(
        IQueryableInjector<TDomainObject> queryableInjector,
        FetchRule<TDomainObject>? fetchRule = null)
    {
        if (queryableInjector is null) throw new ArgumentNullException(nameof(queryableInjector));

        var secureQueryable = this.GetSecureQueryable(fetchRule);

        return queryableInjector.Inject(secureQueryable);
    }

    /// <summary>
    /// Searches for a domain object by condition.
    /// </summary>
    /// <param name="filter">Expression that defines the search condition.</param>
    /// <param name="getNotFoundException">Delegate that returns the exception to throw if the object is not found.</param>
    /// <param name="fetchRule">Paths that define the graph fetch.</param>
    /// <returns>The object found by the condition.</returns>
    /// <exception cref="ArgumentNullException">Argument
    /// <paramref name="filter"/>
    /// or
    /// <paramref name="getNotFoundException"/>
    /// </exception>
    /// <exception cref="InvalidOperationException">More than one object was found for the given condition.</exception>
    public TDomainObject GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        Func<Exception> getNotFoundException,
        FetchRule<TDomainObject>? fetchRule = null)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));
        if (getNotFoundException is null) throw new ArgumentNullException(nameof(getNotFoundException));

        return this.GetObjectBy(filter, false, fetchRule).FromMaybe(getNotFoundException);
    }

    /// <summary>
    /// Searches for a domain object by condition.
    /// </summary>
    /// <param name="lockRole">Instance of <see cref="LockRole"/> that defines the requested lock.</param>
    /// <param name="filter">Expression that defines the search condition.</param>
    /// <param name="fetchRule">Paths that define the graph fetch.</param>
    /// <param name="throwOnNotFound">
    ///   When <c>true</c> is passed, <see cref="BusinessLogicException"/> will be thrown.
    ///   if the object is not found by the condition.
    /// </param>
    /// <returns>Instance of the found object, or null if the object is not found.</returns>
    /// <exception cref="BusinessLogicException">If the throwOnNotFound parameter is true and the object is not found.</exception>
    /// <exception cref="InvalidOperationException">More than one object was found for the given condition.</exception>
    [Obsolete("This method overload will be removed in future versions. Use the other overloads instead.")]
    public TDomainObject? GetObjectBy(
        LockRole lockRole,
        Expression<Func<TDomainObject, bool>> filter,
        FetchRule<TDomainObject>? fetchRule = null,
        bool throwOnNotFound = false) =>
        this.GetObjectBy(lockRole, filter, throwOnNotFound, fetchRule);

    /// <inheritdoc />
    public TDomainObject? GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        bool throwOnNotFound = false,
        FetchRule<TDomainObject>? fetchRule = null) =>
        this.GetObjectBy(LockRole.None, filter, throwOnNotFound, fetchRule);

    public TDomainObject? GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        bool throwOnNotFound,
        Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule) =>
        this.GetObjectBy(filter, throwOnNotFound, buildFetchRule.ToFetchRule());

    /// <summary>
    /// Searches for a domain object by condition.
    /// </summary>
    /// <param name="lockRole">Instance of <see cref="LockRole"/> that defines the requested lock.</param>
    /// <param name="filter">Expression that defines the search condition.</param>
    /// <param name="throwOnNotFound">
    ///   When <c>true</c> is passed, <see cref="BusinessLogicException"/> will be thrown.
    ///   if the object is not found by the condition.
    /// </param>
    /// <param name="fetchRule">Paths that define the graph fetch.</param>
    /// <returns>Instance of the found object, or null if the object is not found.</returns>
    /// <exception cref="BusinessLogicException">If the throwOnNotFound parameter is true and the object is not found.</exception>
    /// <exception cref="InvalidOperationException">More than one object was found for the given condition.</exception>
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

    public virtual void Lock(TDomainObject domainObject, LockRole lockRole) => this.Dal.Lock(domainObject, lockRole);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    public virtual void Refresh(TDomainObject domainObject) => this.Dal.Refresh(domainObject);

    #region Revision

    public virtual TDomainObject GetObjectByRevision(TIdent id, long revision) => this.Dal.GetObjectByRevision(id, revision);

    public virtual long? GetPreviousRevision(TIdent id, long maxRevision) => this.Dal.GetPreviousRevision(id, maxRevision);

    public virtual TDomainObject GetObjectsByPrevRevision(TIdent id)
    {
        var objectsRevisions = this.GetObjectRevisions(id).RevisionInfos.OrderByDescending(info => info.Date).ToList();

        var revision = objectsRevisions
                       .First(() => new ArgumentException($"Object with id:{id} are not found. (Type:{typeof(TDomainObject).Name})"))
                       .RevisionNumber;

        return this.GetObjectByRevision(id, revision);
    }

    public virtual IEnumerable<TDomainObject> GetObjectsByRevision(IEnumerable<TIdent> idCollection, long revision) => this.Dal.GetObjectsByRevision(idCollection, revision);

    public virtual DomainObjectRevision<TIdent> GetObjectRevisions(TIdent identity, Period? period = null) => this.Dal.GetObjectRevisions(identity, period);

    public virtual long? GetPreviousVersion(TIdent id, long maxRevisionNumber) => this.Dal.GetPreviousRevision(id, maxRevisionNumber);

    public virtual IEnumerable<long> GetRevisions(TIdent id) => this.Dal.GetRevisions(id);

    public virtual IReadOnlyList<Tuple<TDomainObject, long>> GetDomainObjectRevisions(TIdent id, int takeCount) => this.Dal.GetDomainObjectRevisions<TDomainObject>(id, takeCount);

    public virtual DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(
        TIdent id,
        Expression<Func<TDomainObject, TProperty>> propertyExpression,
        Period? period = null) =>
        this.Dal.GetPropertyRevisions(id, propertyExpression, period);

    public virtual DomainObjectPropertyRevisions<TIdent, TProperty> GetPropertyChanges<TProperty>(
        TIdent id,
        string propertyName,
        Period? period = null) =>
        this.Dal.GetPropertyRevisions<TProperty>(id, propertyName, period);

    public virtual IDomainObjectPropertyRevisionBase<TIdent, RevisionInfoBase> GetUnTypedPropertyChanges(
        TIdent id,
        string propertyName,
        Period? period = null) =>
        this.Dal.GetUntypedPropertyRevisions(id, propertyName, period);

    public IEnumerable<TIdent> GetIdentiesWithHistory(Expression<Func<TDomainObject, bool>> expression) => this.Dal.GetIdentiesWithHistory(expression);

    /// <summary>
    /// Get current revision. Actual info only after flush
    /// </summary>
    /// <returns></returns>
    public long GetCurrentRevision() => this.Dal.GetCurrentRevision();

    #endregion

}
