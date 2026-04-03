namespace Framework.Validation;

public class ValidationContext<TSource, TValidationMap>(
    IValidator validator,
    int operationContext,
    TSource source,
    IValidationState? parentState,
    TValidationMap map,
    IServiceProvider serviceProvider)
    : ValidationContextBase<TSource>(validator, operationContext, source, parentState), IValidationContext<TSource, TValidationMap>
    where TValidationMap : class
{
    public TValidationMap Map { get; } = map;

    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}
