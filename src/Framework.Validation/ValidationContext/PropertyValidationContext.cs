namespace Framework.Validation;

public class PropertyValidationContext<TSource, TProperty>(
    IValidator validator,
    int operationContext,
    TSource source,
    IValidationState parentState,
    IPropertyValidationMap<TSource, TProperty> map,
    IServiceProvider serviceProvider,
    TProperty value)
    : ValidationContext<TSource, IPropertyValidationMap>(validator, operationContext, source, parentState, map, serviceProvider), IPropertyValidationContext<TSource, TProperty>
{
    public TProperty Value { get; } = value;
}
