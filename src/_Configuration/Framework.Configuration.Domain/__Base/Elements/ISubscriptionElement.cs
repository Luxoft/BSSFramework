namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Интерфейс элемента валидации подписки
    /// </summary>
    public interface ISubscriptionElement
    {
        Subscription Subscription { get; }
    }
}