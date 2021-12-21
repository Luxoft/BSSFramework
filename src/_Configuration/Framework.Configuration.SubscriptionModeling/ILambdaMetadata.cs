using System;

namespace Framework.Configuration.SubscriptionModeling
{
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
        /// <remarks>Значение по умолчанию <see cref="SubscriptionModeling.DomainObjectChangeType.Any"/></remarks>
        DomainObjectChangeType DomainObjectChangeType { get; }

        /// <summary>Получает делегат, исполняющий лямбду.</summary>
        /// <value>Делегат, исполняющий лямбду.</value>
        Func<object, object, object> Lambda { get; }

        /// <summary>
        /// Проверяет корректность модели.
        /// </summary>
        void Validate();
    }
}