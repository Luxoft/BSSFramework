namespace Framework.Validation;

public class IntValueValidator(int min, int max) : IPropertyValidator<object, int>
{
    public ValidationResult GetValidationResult(IPropertyValidationContext<object, int> context) =>
        ValidationResult.FromCondition(
            min <= context.Value && context.Value <= max,
            () => $"The value of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should between {this.GetDesignRange()}");

    private string GetDesignRange()
    {
        if (max == int.MaxValue)
        {
            return min.ToString();
        }

        if (min == int.MinValue)
        {
            return max.ToString();
        }

        return $"{min}-{max}";
    }
}
