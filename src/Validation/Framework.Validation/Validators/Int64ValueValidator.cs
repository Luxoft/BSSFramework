namespace Framework.Validation.Validators;

public class Int64ValueValidator(long min, long max) : IPropertyValidator<object, long>
{
    public ValidationResult GetValidationResult(IPropertyValidationContext<object, long> context) =>
        ValidationResult.FromCondition(
            min <= context.Value && context.Value <= max,
            () => $"The value of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should between {this.GetDesignRange()}");

    private string GetDesignRange()
    {
        if (max == long.MaxValue)
        {
            return min.ToString();
        }

        if (min == long.MinValue)
        {
            return max.ToString();
        }

        return $"{min}-{max}";
    }
}
