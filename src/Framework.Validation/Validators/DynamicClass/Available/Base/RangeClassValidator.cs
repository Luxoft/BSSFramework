using System;
using System.Reflection;

using Framework.Core;

namespace Framework.Validation
{
    public abstract class RangeClassValidator<TProperty, TRange> : IManyPropertyDynamicClassValidator
        where TProperty : struct
    {
        protected abstract Func<Range<TRange>, TProperty, bool> IsValidValueFunc { get; }


        public IPropertyValidator GetValidator(PropertyInfo property, IDynamicSource extendedValidationData)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            var availableValues = extendedValidationData.GetValue<IAvailableValues>(true);

            if (property.PropertyType == typeof(TProperty))
            {
                var availableRange = availableValues.GetAvailableRange<TRange>();

                var propValidatorType = typeof(RangePropertyValidator<,,>).MakeGenericType(property.ReflectedType, typeof(TProperty), typeof(TRange));

                return (IPropertyValidator)Activator.CreateInstance(propValidatorType, availableRange, this.IsValidValueFunc);
            }
            else if (property.PropertyType == typeof(TProperty?))
            {
                var availableRange = availableValues.GetAvailableRange<TRange>();

                var propValidatorType = typeof(NullableRangePropertyValidator<,,>).MakeGenericType(property.ReflectedType, typeof(TProperty), typeof(TRange));

                return (IPropertyValidator)Activator.CreateInstance(propValidatorType, availableRange, this.IsValidValueFunc);
            }
            else
            {
                return null;
            }
        }
    }


    public class RangePropertyValidator<TSource, TProperty, TRange> : IPropertyValidator<TSource, TProperty>
    {
        private readonly Range<TRange> _availableRange;

        private readonly Func<Range<TRange>, TProperty, bool> _isValidValueFunc;

        public RangePropertyValidator(Range<TRange> availableRange, Func<Range<TRange>, TProperty, bool> isValidValueFunc)
        {
            if (availableRange == null) throw new ArgumentNullException(nameof(availableRange));
            if (isValidValueFunc == null) throw new ArgumentNullException(nameof(isValidValueFunc));

            this._availableRange = availableRange;
            this._isValidValueFunc = isValidValueFunc;
        }

        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> context)
        {
            return ValidationResult.FromCondition(this.IsValidValue(context), () =>
                $"{context.GetSourceTypeName()} has {context.Map.Property.Name.ToStartUpperCase()} value was too overflow for a {context.GetPropertyTypeName()}");
        }

        private bool IsValidValue(IPropertyValidationContext<TSource, TProperty> context)
        {
            return this._isValidValueFunc(this._availableRange, context.Value);
        }
    }

    public class NullableRangePropertyValidator<TSource, TProperty, TRange> : RangePropertyValidator<TSource, TProperty?, TRange>
        where TProperty : struct
    {
        public NullableRangePropertyValidator(Range<TRange> availableRange, Func<Range<TRange>, TProperty, bool> isValidValueFunc)
            : base(availableRange, (range, value) => value == null || isValidValueFunc(range, value.Value))
        {
        }
    }
}