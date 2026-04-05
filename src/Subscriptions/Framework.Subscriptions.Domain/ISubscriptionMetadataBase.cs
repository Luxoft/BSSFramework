namespace Framework.Subscriptions.Domain;

public interface ISubscriptionMetadataBase
{
    /// <summary>
    ///     Получает имя подписки.
    /// </summary>
    /// <value>Имя подписки.</value>
    string Name { get; }
}
