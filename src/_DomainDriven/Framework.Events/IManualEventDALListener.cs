using Framework.DomainDriven.BLL;

namespace Framework.Events
{
    /// <summary>
    /// Потребитель DAL-евентов с возможностью их ручной эмуляции
    /// </summary>
    public interface IManualEventDALListener<in TPersistentDomainObjectBase> : IDALListener, IPersistentDomainObjectBaseTypeContainer
    {
        /// <summary>
        /// Получение контейнера для ручного вызова евента
        /// </summary>
        /// <typeparam name="TDomainObject"></typeparam>
        /// <returns></returns>
        IForceEventContainer<TDomainObject, EventOperation> GetForceEventContainer<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;
    }
}
