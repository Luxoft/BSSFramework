using Framework.DomainDriven.BLL;

public interface IEventSubscriber : IDBSessionEventListener
{
    /// <summary>
    /// Подписка на евенты
    /// </summary>
    void Subscribe();
}
