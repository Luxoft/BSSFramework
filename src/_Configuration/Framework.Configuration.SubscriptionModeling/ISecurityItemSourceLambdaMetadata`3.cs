using Framework.SecuritySystem;

namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
/// Представляет интерфейс экземпляра конфигурации лямбда выражения подписки.
/// </summary>
/// <typeparam name="TContext">Текущий контекст бизнес-логики.</typeparam>
/// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
/// <typeparam name="TResult">Тип возвращаемого выражением значения.</typeparam>
/// <seealso cref="Framework.Configuration.SubscriptionModeling.ISecurityItemSourceLambdaMetadata" />
public interface ISecurityItemSourceLambdaMetadata<in TContext, in TDomainObject, out TResult> : ISecurityItemSourceLambdaMetadata
        where TResult : ISecurityContext
{
}
