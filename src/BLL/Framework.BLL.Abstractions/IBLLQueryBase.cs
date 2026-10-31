using System.Linq.Expressions;

using Anch.GenericQueryable.Fetching;
using Anch.OData.Domain;

using Framework.Application;
using Framework.BLL.Domain.Models;
using Framework.Database;

namespace Framework.BLL;

public interface IBLLQueryBase<TDomainObject> : IBLLSimpleQueryBase<TDomainObject>
{
    /// <summary>
    /// Getting IQueryable with security
    /// </summary>
    /// <param name="fetchRule">Properties to fetch</param>
    /// /// <param name="lockRole">lockRole</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetSecureQueryable(FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    /// <summary>
    /// Getting IQueryable with security
    /// </summary>
    /// <param name="buildFetchRule">Properties to fetch</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetSecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);


    /// <summary>
    /// Getting IQueryable without security
    /// </summary>
    /// <param name="fetchRule">Properties to fetch</param>
    /// <param name="lockRole">lockRole</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable(FetchRule<TDomainObject> fetchRule, LockRole lockRole = LockRole.None);

    /// <summary>
    /// Getting IQueryable without security
    /// </summary>
    /// <param name="buildFetchRule">Properties to fetch</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);

    SelectOperationResult<TDomainObject> GetObjectsByOData(SelectOperation<TDomainObject> selectOperation, FetchRule<TDomainObject>? fetchRule = null);

    SelectOperationResult<TDomainObject> GetObjectsByOData(
        SelectOperation<TDomainObject> selectOperation,
        Expression<Func<TDomainObject, bool>> filter,
        FetchRule<TDomainObject>? fetchRule = null);

    SelectOperationResult<TDomainObject> GetObjectsByOData(
        SelectOperation<TDomainObject> selectOperation,
        IDomainObjectFilterModel<TDomainObject> filter,
        FetchRule<TDomainObject>? fetchRule = null);

    List<TDomainObject> GetFullList(FetchRule<TDomainObject>? fetchRule = null);

    List<TDomainObject> GetFullList(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);

    List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule);

    List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule);

    /// <summary>
    /// Searches for a domain object by condition.
    /// </summary>
    /// <param name="filter">Expression that defines the search condition.</param>
    /// <param name="throwOnNotFound">
    ///   When <c>true</c> is passed, <see cref="BusinessLogicException"/> will be thrown.
    ///   if the object is not found by the condition.
    /// </param>
    /// <param name="fetchRule">Paths that define the graph fetch.</param>
    /// <returns>Instance of the found object, or null if the object is not found.</returns>
    /// <exception cref="BusinessLogicException">If the throwOnNotFound parameter is true and the object is not found.</exception>
    /// <exception cref="InvalidOperationException">More than one object was found for the given condition.</exception>
    TDomainObject? GetObjectBy(Expression<Func<TDomainObject, bool>> filter, bool throwOnNotFound = false, FetchRule<TDomainObject>? fetchRule = null);

    TDomainObject? GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        bool throwOnNotFound,
        Func<PropertyFetchRule<TDomainObject>, FetchRule<TDomainObject>> buildFetchRule);
}
