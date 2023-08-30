using System.Reflection;

using Framework.Core;

namespace Framework.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class AnyElementsValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator()
    {
        return AnyElementsValidator.Value;
    }
}

public class AnyElementsValidator : IDynamicPropertyValidator
{
    public IPropertyValidator GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var elementType = property.PropertyType.GetCollectionElementType();

        return (IPropertyValidator)Activator.CreateInstance(typeof(AnyElementsValidator<>).MakeGenericType(elementType));
    }

    public static AnyElementsValidator Value { get; } = new AnyElementsValidator();
}

public class AnyElementsValidator<TElement> : IPropertyValidator<object, IEnumerable<TElement>>
{
    public ValidationResult GetValidationResult(IPropertyValidationContext<object, IEnumerable<TElement>> context)
    {
        var value = context.Value;

        return ValidationResult.FromCondition(null == value || value.Any(),
                                              () => $"Collection {context.GetPropertyTypeName()} of {context.GetSourceTypeName()} can't be empty");
    }
}
