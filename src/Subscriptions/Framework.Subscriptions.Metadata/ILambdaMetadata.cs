using System.Runtime.InteropServices.JavaScript;

using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

/// <summary>
///     Представляет интерфейс экземпляра конфигурации лямбда выражения подписки.
/// </summary>
public interface ILambdaMetadata
{
    /// <summary>
    /// Получает тип изменения доменного объекта для которого должна срабатывать подписка.
    /// </summary>
    /// <value>
    /// тип изменения доменного объекта для которого должна срабатывать подписка.
    /// </value>
    /// <remarks>Значение по умолчанию <see cref="JSType.Any"/></remarks>
    DomainObjectChangeType DomainObjectChangeType { get; }

    /// <summary>Получает делегат, исполняющий лямбду.</summary>
    /// <value>Делегат, исполняющий лямбду.</value>
    Delegate? Lambda { get; }
}
