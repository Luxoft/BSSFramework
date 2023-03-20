using System;
using System.Linq;
using System.Reflection;

using Framework.Core;

namespace Framework.Validation;

public class MaxLengthValidator : IDynamicPropertyValidator
{
    private readonly int _maxLength;

    public MaxLengthValidator(int maxLength)
    {
        if (maxLength < 0) { throw new ArgumentOutOfRangeException(nameof(maxLength)); }

        this._maxLength = maxLength;
    }

    public IPropertyValidator GetValidator(PropertyInfo property, IDynamicSource extendedValidationData)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

        var validatorType = this.GetValidatorType(property);

        var ctor = validatorType.GetConstructors().Single();

        return (IPropertyValidator)ctor.Invoke(new object[] { this._maxLength });
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
        private readonly int _maxLength;

        public StringMaxLengthValidator(int maxLength)
        {
            if (maxLength < 0) { throw new ArgumentOutOfRangeException(nameof(maxLength)); }

            this._maxLength = maxLength;
        }

        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, string> context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return ValidationResult.FromCondition(
                                                  context.Value == null || context.Value.Length <= this._maxLength,
                                                  () => $"The length of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should not be more than {this._maxLength}");
        }
    }

    public class BinaryMaxLengthValidator<TSource> : IPropertyValidator<TSource, byte[]>
    {
        private readonly int _maxLength;

        public BinaryMaxLengthValidator(int maxLength)
        {
            this._maxLength = maxLength;
        }

        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, byte[]> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return ValidationResult.FromCondition(
                                                  context.Value == null || context.Value.Length <= this._maxLength,
                                                  () => $"The length of {context.GetPropertyName()} property of {context.GetSourceTypeName()} should not be more than {this._maxLength}");
        }
    }
}
