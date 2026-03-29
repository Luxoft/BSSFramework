using SecuritySystem;

namespace Framework.Subscriptions;

/// <summary>
/// Представляет интерфейс экземпляра конфигурации лямбда выражения подписки.
/// </summary>
/// <typeparam name="TContext">Текущий контекст бизнес-логики.</typeparam>
/// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
/// <typeparam name="TResult">Тип возвращаемого выражением значения.</typeparam>
/// <seealso cref="ISecurityItemSourceLambdaMetadata" />
public interface ISecurityItemSourceLambdaMetadata<in TContext, in TDomainObject, out TResult> : ISecurityItemSourceLambdaMetadata
        where TResult : ISecurityContext
{
}
