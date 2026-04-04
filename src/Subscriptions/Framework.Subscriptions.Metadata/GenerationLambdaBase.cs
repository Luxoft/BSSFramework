namespace Framework.Subscriptions.Metadata;

/// <inheritdoc />
public abstract class GenerationLambdaBase<TDomainObject> :
    LambdaMetadata<TDomainObject, IEnumerable<NotificationMessageGenerationInfo>>
    where TDomainObject : class;
