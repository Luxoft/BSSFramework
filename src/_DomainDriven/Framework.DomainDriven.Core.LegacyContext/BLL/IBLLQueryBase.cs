using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Framework.DomainDriven.Lock;
using Framework.Exceptions;
using Framework.OData;

using GenericQueryable.Fetching;

namespace Framework.DomainDriven.BLL;

public interface IBLLQueryBase<TDomainObject> : IBLLSimpleQueryBase<TDomainObject>
{
    /// <summary>
    /// Получение IQueryable с учётом безопасности
    /// </summary>
    /// <param name="fetchRule">Подгружаемые свойства</param>
    /// /// <param name="lockRole">lockRole</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetSecureQueryable(FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    /// <summary>
    /// Получение IQueryable с учётом безопасности
    /// </summary>
    /// <param name="buildFetchRule">Подгружаемые свойства</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetSecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);

    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <param name="fetchRule">Подгружаемые свойства</param>
    /// <param name="lockRole">lockRole</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable(FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <param name="buildFetchRule">Подгружаемые свойства</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable(Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);


    SelectOperationResult<TDomainObject> GetObjectsByOData(SelectOperation selectOperation, FetchRule<TDomainObject>? fetchRule = null);

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

    List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);

    List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, FetchRule<TDomainObject>? fetchRule = null, LockRole lockRole = LockRole.None);

    List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);


    SelectOperationResult<TProjection> GetObjectsByOData<TProjection>(
        SelectOperation<TProjection> selectOperation,
        Expression<Func<TDomainObject, TProjection>> projectionSelector);

    /// <summary>
    /// Выполняет поиск доменного объекта по условию.
    /// </summary>
    /// <param name="filter">Выражение, определяющее условие поиска.</param>
    /// <param name="throwOnNotFound">
    ///   Когда передано <c>true</c> будет выброшено <see cref="BusinessLogicException"/>.
    ///   если объект по условию не найден.
    /// </param>
    /// <param name="fetchRule">Пути, определяющие выгрузку графа.</param>
    /// <returns>Экземпляр найденного объекта или null, если объект не найден.</returns>
    /// <exception cref="BusinessLogicException">Если параметр throwOnNotFound задан как true и объект не найден.</exception>
    /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
    [return: NotNullIfNotNull(nameof(throwOnNotFound))]
    TDomainObject? GetObjectBy(Expression<Func<TDomainObject, bool>> filter, bool throwOnNotFound = false, FetchRule<TDomainObject>? fetchRule = null);

    [return: NotNullIfNotNull(nameof(throwOnNotFound))]
    TDomainObject? GetObjectBy(
        Expression<Func<TDomainObject, bool>> filter,
        bool throwOnNotFound,
        Func<PropertyFetchRule<TDomainObject>, PropertyFetchRule<TDomainObject>> buildFetchRule);
}
