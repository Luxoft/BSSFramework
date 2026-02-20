using System.Reflection;

using CommonFramework;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

public abstract class RangeClassValidator<TProperty, TRange> : IManyPropertyDynamicClassValidator
        where TProperty : struct
{
    protected abstract Func<Range<TRange>, TProperty, bool> IsValidValueFunc { get; }


    public IPropertyValidator? GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var availableValues = serviceProvider.GetRequiredService<IAvailableValues>();

        if (property.PropertyType == typeof(TProperty))
        {
            var availableRange = availableValues.GetAvailableRange<TRange>();

            var propValidatorType = typeof(RangePropertyValidator<,,>).MakeGenericType(property.ReflectedType!, typeof(TProperty), typeof(TRange));

            return serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IPropertyValidator>(propValidatorType, availableRange, this.IsValidValueFunc);
        }
        else if (property.PropertyType == typeof(TProperty?))
        {
            var availableRange = availableValues.GetAvailableRange<TRange>();

            var propValidatorType = typeof(NullableRangePropertyValidator<,,>).MakeGenericType(property.ReflectedType!, typeof(TProperty), typeof(TRange));

            return serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IPropertyValidator>(propValidatorType, availableRange, this.IsValidValueFunc);
        }
        else
        {
            return null;
        }
    }
}


public class RangePropertyValidator<TSource, TProperty, TRange>(Range<TRange> availableRange, Func<Range<TRange>, TProperty, bool> isValidValueFunc)
    : IPropertyValidator<TSource, TProperty>
{
    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> context)
    {
        return ValidationResult.FromCondition(
            this.IsValidValue(context),
            () =>
                $"{context.GetSourceTypeName()} has {context.Map.Property.Name.ToStartUpperCase()} value was too overflow for a {context.GetPropertyTypeName()}");
    }

    private bool IsValidValue(IPropertyValidationContext<TSource, TProperty> context)
    {
        return isValidValueFunc(availableRange, context.Value);
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
