using Framework.DomainDriven.BLL;

namespace Framework.Events
{
    /// <summary>
    /// Интерфейс для описания правил подписок на доменные евенты
    /// </summary>
    /// <typeparam name="TBLLContext"></typeparam>
    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    public interface IEventsSubscriptionManager<out TBLLContext, TPersistentDomainObjectBase> : IBLLContextContainer<TBLLContext>
        where TPersistentDomainObjectBase : class
        where TBLLContext : class, IBLLOperationEventContext<TPersistentDomainObjectBase>
    {
        /// <summary>
        /// Подписка на события
        /// </summary>
        void Subscribe();
    }
}