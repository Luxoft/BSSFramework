using System.Reflection;

namespace Framework.Validation;

public class MaxLengthValidator : IDynamicPropertyValidator
{
    private readonly int maxLength;

    public MaxLengthValidator(int maxLength)
    {
        if (maxLength < 0) { throw new ArgumentOutOfRangeException(nameof(maxLength)); }

        this.maxLength = maxLength;
    }

    public IPropertyValidator GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var validatorType = this.GetValidatorType(property);

        var ctor = validatorType.GetConstructors().Single();

        return (IPropertyValidator)ctor.Invoke([this.maxLength]);
    }

    public Type GetValidatorType(PropertyInfo property)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));

        if (property.PropertyType == typeof(string))
        {
            return typeof(StringMaxLengthValidator<>).MakeGenericType(property.ReflectedType);
        }
        else if (property.PropertyType == typeof(byte[]))
        {
            return typeof(BinaryMaxLengthValidator<>).MakeGenericType(property.ReflectedType);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(property));
        }
    }

    public class StringMaxLengthValidator<TSource> : IPropertyValidator<TSource, string>
    {
        private readonly int maxLength;

        public StringMaxLengthValidator(int maxLength)
        {
            if (maxLength < 0) { throw new ArgumentOutOfRangeException(nameof(maxLength)); }

            this.maxLength = maxLength;
        }

        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, string> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return ValidationResult.FromCondition(
                                                  context.Value == null || context.Value.Length <= this.maxLength,
                                                  () => $"The length of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should not be more than {this.maxLength}");
        }
    }

    public class BinaryMaxLengthValidator<TSource>(int maxLength) : IPropertyValidator<TSource, byte[]>
    {
        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, byte[]> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ValidationResult.FromCondition(
                                                  context.Value == null || context.Value.Length <= maxLength,
                                                  () => $"The length of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should not be more than {maxLength}");
        }
    }
}
