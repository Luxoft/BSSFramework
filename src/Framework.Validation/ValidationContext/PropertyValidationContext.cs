namespace Framework.Validation;

public class PropertyValidationContext<TSource, TProperty> : ValidationContext<TSource, IPropertyValidationMap>, IPropertyValidationContext<TSource, TProperty>
{
    public PropertyValidationContext(IValidator validator, int operationContext, TSource source, IValidationState parentState, IPropertyValidationMap<TSource, TProperty> map, IServiceProvider serviceProvider, TProperty value)
            : base(validator, operationContext, source, parentState, map, serviceProvider)
    {
        this.Value = value;
    }


    public TProperty Value { get; }
}
