using Framework.Core;

namespace Framework.SecuritySystem
{
    /// <summary>
    /// Провайдер доступа к объектам
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public interface ISecurityProvider<TDomainObject>
    {
        /// <summary>
        /// Добавление Queryable-фильтрации к поиску доступных объектов
        /// </summary>
        /// <param name="queryable"></param>
        /// <returns></returns>
        IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable);

        /// <summary>
        /// Проверка наличия доступа на объект для текущего пользователя с расширенной информацией
        /// </summary>
        /// <param name="domainObject"></param>
        /// <returns></returns>
        AccessResult GetAccessResult(TDomainObject domainObject)
        {
            return this.HasAccess(domainObject)
                       ? AccessResult.AccessGrantedResult.Default
                       : AccessResult.AccessDeniedResult.Create(domainObject);
        }

        /// <summary>
        /// Проверка наличия доступа на объект для текущего пользователя
        /// </summary>
        /// <param name="domainObject"></param>
        /// <returns></returns>
        bool HasAccess(TDomainObject domainObject);

        /// <summary>
        /// Получение списка пользователей имеющих доступ к обьекту
        /// </summary>
        /// <param name="domainObject"></param>
        /// <returns></returns>
        UnboundedList<string> GetAccessors(TDomainObject domainObject);
    }
}
