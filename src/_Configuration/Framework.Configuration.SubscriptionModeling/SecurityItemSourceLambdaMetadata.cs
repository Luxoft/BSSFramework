﻿using Framework.Authorization.Notification;
using Framework.SecuritySystem;

namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
///     Класс экземпляра конфигурации лямбда-выражения SecurityItemSource.
/// </summary>
/// <typeparam name="TContext">Текущий контекст бизнес-логики.</typeparam>
/// <typeparam name="TDomainObject">Тип доменного объекта.</typeparam>
/// <typeparam name="TResult">Тип возвращаемого выражением значения.</typeparam>
public abstract class SecurityItemSourceLambdaMetadata<TContext, TDomainObject, TResult> : LambdaMetadata<TContext, TDomainObject, IEnumerable<TResult>>, ISecurityItemSourceLambdaMetadata<TContext, TDomainObject, TResult>
        where TDomainObject : class
        where TResult : ISecurityContext
{
    /// <inheritdoc />
    public virtual NotificationExpandType ExpandType { get; protected set; }

    /// <inheritdoc />
    public Type AuthDomainType { get; } = typeof(TResult);
}
