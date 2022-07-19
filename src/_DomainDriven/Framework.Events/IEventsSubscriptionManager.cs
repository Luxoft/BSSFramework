namespace Framework.Events
{
    /// <summary>
    /// Интерфейс для описания правил подписок на доменные евенты
    /// </summary>
    public interface IEventsSubscriptionManager
    {
        /// <summary>
        /// Подписка на события
        /// </summary>
        void Subscribe();
    }
}
