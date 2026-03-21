namespace Framework.Validation;

public class AllPropertiesValidator<TProperty>(int operationContext) : IPropertyValidator<object, TProperty>
    where TProperty : class
{
    public ValidationResult GetValidationResult(IPropertyValidationContext<object, TProperty> context) => context.Validator.GetValidationResult(context.Value, operationContext);
}
