using System.Reflection;

using Framework.Core;
using Framework.Restriction;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

public class DefaultStringMaxLengthValidator : IManyPropertyDynamicClassValidator
{
    public IPropertyValidator? GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var availableValues = serviceProvider.GetRequiredService<IAvailableValues>();

        if (property.PropertyType != typeof(string) || property.HasAttribute<MaxLengthAttribute>()
                                                    || property.HasAttribute<MaxLengthValidatorAttribute>())
        {
            return null;
        }

        return new MaxLengthValidator(availableValues.GetAvailableSize<string>());
    }


    public static DefaultStringMaxLengthValidator Value { get; } = new DefaultStringMaxLengthValidator();
}
