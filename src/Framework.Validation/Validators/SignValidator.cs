using CommonFramework;

using Framework.Core;
using Framework.Persistent;

namespace Framework.Validation;

/// <summary>
/// Represents numeric value sign validator that used by domain object properties validation
/// </summary>
public class SignValidator : IPropertyValidator<object, object>
{
    /// <summary>
    /// Numeric property value acceptable sign type
    /// </summary>
    protected readonly SignType ExpectedPropertyValueSignType;

    /// <summary>
    /// Initializes new validator instance using expected numeric value sign type
    /// </summary>
    /// <param name="expectedPropertyValueSignType">Numeric property value acceptable sign type</param>
    public SignValidator(SignType expectedPropertyValueSignType)
    {
        this.ExpectedPropertyValueSignType = expectedPropertyValueSignType;
    }

    /// <summary>
    /// Runs validation using sign type passed to ctor end returns validation results that can be verified by caller
    /// </summary>
    /// <param name="context">Validation context that contains all necessary data</param>
    /// <returns>Validation result to be verified by caller</returns>
    public virtual ValidationResult GetValidationResult(IPropertyValidationContext<object, object> context)
    {
        return this.GetValidateResult(this.ExpectedPropertyValueSignType, context);
    }

    /// <summary>
    /// Runs validation using sign type specified end returns validation results that can be verified by caller
    /// </summary>
    /// <param name="expectedSignType">Numeric property value acceptable sign type</param>
    /// <param name="context">Validation context that contains all necessary data</param>
    /// <returns>Validation result to be verified by caller</returns>
    protected ValidationResult GetValidateResult(SignType expectedSignType, IPropertyValidationContext<object, object> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));


        if (context.Value == null)
        {
            return ValidationResult.Success;
        }

        var sign = Convert.ToDecimal(context.Value).CompareTo(0M);

        var name = (context.GetSource() as IVisualIdentityObject).Maybe(z => z.Name, string.Empty);

        if (sign < 0 && !expectedSignType.HasFlag(SignType.Negative) && !expectedSignType.HasFlag(SignType.Zero))
        {
            return CreateErrorResult(context, "The {0} of {1}{2} must be greater than 0", name);
        }

        if (sign != 0 && expectedSignType == SignType.Zero)
        {
            return CreateErrorResult(context, "The {0} of {1}{2} must be 0", name);
        }

        if (sign < 0 && !expectedSignType.HasFlag(SignType.Negative))
        {
            return CreateErrorResult(context, "The {0} of {1}{2} must be greater or equal 0", name);
        }

        if (sign == 0 && !expectedSignType.HasFlag(SignType.Zero))
        {
            return CreateErrorResult(context, "The {0} of {1}{2} can not be equal 0", name);
        }

        if (sign > 0 && !expectedSignType.HasFlag(SignType.Positive))
        {
            return CreateErrorResult(context, "The {0} of {1}{2} must be less than 0", name);
        }

        return ValidationResult.Success;
    }

    private static ValidationResult CreateErrorResult(IPropertyValidationContext<object, object> context, string template, string name)
    {
        return ValidationResult.CreateError(
                                            template,
                                            context.GetPropertyName(),
                                            context.GetSourceTypeName(),
                                            name.IsNullOrWhiteSpace() ? string.Empty : $":'{name}'");
    }
}
