using System;

namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
///     Базовый класс экземпляра метаданных Code first подписки.
/// TODO: rename to CodeFirstSubscriptionBase
/// </summary>
/// <typeparam name="TContext">Текущий контекст бизнес-логики.</typeparam>
/// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
/// <typeparam name="TTemplate">Тип Razor шаблона.</typeparam>
/// <seealso cref="ISubscriptionMetadata" />
[Obsolete("Use SubscriptionWithCustomModelMetadata")]
public abstract class SubscriptionMetadata<TContext, TDomainObject, TTemplate> : SubscriptionWithCustomModelMetadata<TContext, TDomainObject, TDomainObject, TTemplate>
        where TDomainObject : class
        where TTemplate : IRazorTemplate
{
}
