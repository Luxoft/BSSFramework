namespace Framework.Configuration.SubscriptionModeling;

/// <summary>
///     Определяет интерфейс экземпляра метаданных Code first подписки.
/// TODO: rename to ICodeFirstSubscription
/// </summary>
public interface ISubscriptionMetadata
{
    /// <summary>
    ///     Получает код подписки.
    /// </summary>
    /// <value>Код подписки.</value>
    string Code { get; }

    /// <summary>
    ///     Получает имя отправителя уведомления по подписке.
    /// </summary>
    /// <value>
    ///     Имя отправителя уведомления по подписке.
    /// </value>
    string SenderName { get; }

    /// <summary>
    ///     Получает адрес электронной почты отправителя уведомления по подписке.
    /// </summary>
    /// <value>
    ///     Адрес электронной почты отправителя уведомления по подписке.
    /// </value>
    string SenderEmail { get; }

    /// <summary>
    ///     Получает признак необходимости отправки адресату(ам) индивидуального или консолидированного письма
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо отправлять индивидуальные письма; в противном случае, <c>false</c>.
    /// </value>
    bool SendIndividualLetters { get; }

    /// <summary>
    ///     Получает признак исключения пользователя из текущей рассылки.
    /// </summary>
    /// <value>
    ///     <c>true</c> если пользователя необходимо исключить; в противном случае, <c>false</c>.
    /// </value>
    bool ExcludeCurrentUser { get; }

    /// <summary>
    ///     Получает признак того, что к письму необходимо прикрепить вложения из шаблона.
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо прикрепить вложения; в противном случае, <c>false</c>.
    /// </value>
    bool IncludeAttachments { get; }

    /// <summary>
    ///     Получае признак того, что уведомление можно отправлять с пустым списком получателей
    ///     (например, в случае тестирования).
    /// </summary>
    /// <value>
    ///     <c>true</c> если уведомление можно отправлять с пустым списком получателей;
    ///     в противном случае, <c>false</c>.
    /// </value>
    bool AllowEmptyListOfRecipients { get; }

    /// <summary>
    ///     Получает тип доменного объекта подписки.
    /// </summary>
    /// <value>
    ///     Тип доменного объекта.
    /// </value>
    Type DomainObjectType { get; }

    /// <summary>
    ///     Получает тип Razor шаблона уведомления.
    /// </summary>
    /// <value>
    ///     Тип Razor шаблона уведомления.
    /// </value>
    Type MessageTemplateType { get; }

    /// <summary>
    ///     Получает тип класса, реализующего интерфейс <see cref="ISubscriptionMetadata" />.
    /// </summary>
    /// <value>
    ///     Тип класса, реализующего интерфейс <see cref="ISubscriptionMetadata" />.
    /// </value>
    Type MetadataType { get; }

    /// <summary>
    ///     Получает способ комбинации адресатов рассылки по SubBusinessRoles и Generation.
    /// </summary>
    /// <value>
    ///     Способ комбинации адресатов рассылки по SubBusinessRoles и Generation..
    /// </value>
    RecepientsSelectorMode RecepientsSelectorMode { get; }

    /// <summary>
    ///     Получает тип расширения прав по дереву.
    /// </summary>
    /// <value>
    ///     Тип расширения прав по дереву.
    /// </value>

    /// <summary>
    ///     Получает идентификаторы бизнес-ролей.
    /// </summary>
    /// <value>
    ///     Идентификаторы бизнес-ролей..
    /// </value>
    IEnumerable<Guid> SubBusinessRoleIds { get; }

    /// <summary>
    ///     Возвращает экземпляр лямбда-выражения Condition.
    /// </summary>
    /// <returns>Экземпляр лямбда-выражения Condition.</returns>
    ILambdaMetadata GetConditionLambda();

    /// <summary>
    ///     Возвращает экземпляр лямбда-выражения Generation.
    /// </summary>
    /// <returns>Экземпляр лямбда-выражения Generation.</returns>
    ILambdaMetadata GetGenerationLambda();

    /// <summary>
    ///     Возвращает экземпляр лямбда-выражения Attachment.
    /// </summary>
    /// <returns>Экземпляр лямбда-выражения Attachment.</returns>
    ILambdaMetadata GetAttachmentLambda();

    /// <summary>
    ///     Возвращает экземпляр лямбда-выражения Generation.
    /// </summary>
    /// <returns>Экземпляр лямбда-выражения Generation.</returns>
    ILambdaMetadata GetCopyGenerationLambda();

    /// <summary>
    ///     Возвращает экземпляр лямбда-выражения Generation для определение replyTo.
    /// </summary>
    /// <returns>Экземпляр лямбда-выражения Generation.</returns>
    ILambdaMetadata GetReplyToGenerationLambda();


    /// <summary>
    ///     Возвращает коллекцию экземпляров лямбда-выражения SecurityItemSource.
    /// </summary>
    /// <returns>Коллекция экземпляров SecurityItemSource.</returns>
    IEnumerable<ISecurityItemSourceLambdaMetadata> GetSecurityItemSourceLambdas();

    /// <summary>
    /// Проверяет корректность модели.
    /// </summary>
    void Validate();
}
