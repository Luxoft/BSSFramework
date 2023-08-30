using Framework.Authorization.Notification;

namespace Framework.Configuration.SubscriptionModeling;

/// <inheritdoc />
public interface ISecurityItemSourceLambdaMetadata : ILambdaMetadata
{
    /// <summary>
    ///     Получает тип расширения прав по дереву.
    /// </summary>
    /// <value>
    ///     Тип расширения прав по дереву.
    /// </value>
    NotificationExpandType ExpandType { get; }

    /// <summary>Получает тип доменного типа авторизации для типизированного контекста.</summary>
    /// <value>Тип доменного типа авторизации для типизированного контекста.</value>
    Type AuthDomainType { get; }
}
