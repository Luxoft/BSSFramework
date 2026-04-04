namespace Framework.Subscriptions.Metadata;

public abstract class ConditionLambdaBase<TDomainObject> : LambdaMetadata<TDomainObject, bool>
    where TDomainObject : class;
