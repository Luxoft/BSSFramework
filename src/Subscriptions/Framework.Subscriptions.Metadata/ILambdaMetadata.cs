namespace Framework.Subscriptions.Metadata;

/// <summary>
///     Представляет интерфейс экземпляра конфигурации лямбда выражения подписки.
/// </summary>
public interface ILambdaMetadata
{
    /// <summary>Получает делегат, исполняющий лямбду.</summary>
    /// <value>Делегат, исполняющий лямбду.</value>
    Delegate? Lambda { get; }
}
