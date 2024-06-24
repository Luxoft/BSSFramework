using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.Exceptions;
using Framework.OData;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.BLL;

public interface IBLLQueryBase<TDomainObject> : IBLLSimpleQueryBase<TDomainObject>
{
    /// <summary>
    /// Получение IQueryable с учётом безопасности
    /// </summary>
    /// <param name="fetchContainer">Подгружаемые свойства</param>
    /// /// <param name="lockRole">lockRole</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetSecureQueryable(IFetchContainer<TDomainObject> fetchContainer = null, LockRole lockRole = LockRole.None);

    /// <summary>
    /// Получение IQueryable с учётом безопасности
    /// </summary>
    /// <param name="firstFetch">Первое подгружаемое свойство</param>
    /// <param name="otherFetchs">Прочие подгружаемые свойства</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetSecureQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    /// <summary>
    /// Получение IQueryable с учётом безопасности
    /// </summary>
    /// <param name="fetchs">Подгружаемые свойства</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetSecureQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);

    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <param name="lockRole">Тип блокировки</param>
    /// <param name="fetchContainer">Подгружаемые свойства</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable(IFetchContainer<TDomainObject> fetchContainer, LockRole lockRole = LockRole.None);

    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <param name="lockRole">Тип блокировки</param>
    /// <param name="fetchContainer">Подгружаемые свойства</param>
    /// <returns></returns>
    [Obsolete("Use other overload method with LockRole parameter")]
    IQueryable<TDomainObject> GetUnsecureQueryable(LockRole lockRole, IFetchContainer<TDomainObject> fetchContainer = null);

    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <param name="firstFetch">Первое подгружаемое свойство</param>
    /// <param name="otherFetchs">Прочие подгружаемые свойства</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    /// <summary>
    /// Получение IQueryable без учёта безопасности
    /// </summary>
    /// <param name="fetchs">Подгружаемые свойства</param>
    /// <returns></returns>
    IQueryable<TDomainObject> GetUnsecureQueryable(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


    SelectOperationResult<TDomainObject> GetObjectsByOData(SelectOperation selectOperation, IFetchContainer<TDomainObject> fetchContainer = null);


    SelectOperationResult<TDomainObject> GetObjectsByOData(SelectOperation<TDomainObject> selectOperation, IFetchContainer<TDomainObject> fetchContainer = null);

    SelectOperationResult<TDomainObject> GetObjectsByOData(SelectOperation<TDomainObject> selectOperation, Expression<Func<TDomainObject, bool>> filter, IFetchContainer<TDomainObject> fetchContainer = null);

    SelectOperationResult<TDomainObject> GetObjectsByOData(SelectOperation<TDomainObject> selectOperation, IDomainObjectFilterModel<TDomainObject> filter, IFetchContainer<TDomainObject> fetchContainer = null);

    List<TDomainObject> GetFullList(IFetchContainer<TDomainObject> fetchContainer = null);

    List<TDomainObject> GetFullList(Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    List<TDomainObject> GetFullList(IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


    List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, IFetchContainer<TDomainObject> fetchContainer = null, LockRole lockRole = LockRole.None);

    List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    List<TDomainObject> GetListBy(IDomainObjectFilterModel<TDomainObject> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);

    List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, IFetchContainer<TDomainObject> fetchContainer = null, LockRole lockRole = LockRole.None);

    List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    List<TDomainObject> GetListBy(Expression<Func<TDomainObject, bool>> filter, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


    SelectOperationResult<TProjection> GetObjectsByOData<TProjection>(SelectOperation<TProjection> selectOperation, Expression<Func<TDomainObject, TProjection>> projectionSelector);

    /// <summary>
    /// Выполняет поиск доменного объекта по условию.
    /// </summary>
    /// <param name="filter">Выражение, определяющее условие поиска.</param>
    /// <param name="throwOnNotFound">
    ///   Когда передано <c>true</c> будет выброшено <see cref="BusinessLogicException"/>.
    ///   если объект по условию не найден.
    /// </param>
    /// <param name="fetchContainer">Пути, определяющие выгрузку графа.</param>
    /// <returns>Экземпляр найденного объекта или null, если объект не найден.</returns>
    /// <exception cref="BusinessLogicException">Если параметр throwOnNotFound задан как true и объект не найден.</exception>
    /// <exception cref="InvalidOperationException">По заданному условию найден более чем один объект.</exception>
    TDomainObject GetObjectBy(Expression<Func<TDomainObject, bool>> filter, bool throwOnNotFound = false, IFetchContainer<TDomainObject> fetchContainer = null);

    TDomainObject GetObjectBy(Expression<Func<TDomainObject, bool>> filter, bool throwOnNotFound, Expression<Action<IPropertyPathNode<TDomainObject>>> firstFetch, params Expression<Action<IPropertyPathNode<TDomainObject>>>[] otherFetchs);

    TDomainObject GetObjectBy(Expression<Func<TDomainObject, bool>> filter, bool throwOnNotFound, IEnumerable<Expression<Action<IPropertyPathNode<TDomainObject>>>> fetchs);


#nullable enable

    /// <summary>
    /// Get Queryable by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    IQueryable<TProjection> GetQueryable<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Single or default by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    TProjection? SingleOrDefault<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Single by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    TProjection Single<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Count by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    int Count<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Future query by Specification https://github.com/NikitaEgorov/nuSpec
    /// NOT SUPPORTED NOW!
    /// </summary>
    INuFutureEnumerable<TProjection> GetFuture<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Future value by Specification https://github.com/NikitaEgorov/nuSpec
    /// NOT SUPPORTED NOW!
    /// </summary>
    INuFutureValue<TProjection> GetFutureValue<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Future count by Specification https://github.com/NikitaEgorov/nuSpec
    /// NOT SUPPORTED NOW!
    /// </summary>
    INuFutureValue<int> GetFutureCount<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get list by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    IList<TProjection> GetList<TProjection>(Specification<TDomainObject, TProjection> specification);

#nullable disable
}

public interface IBLLBase<out TBLLContext, TDomainObject> : IBLLQueryBase<TDomainObject>, IBLLContextContainer<TBLLContext>
{
}
