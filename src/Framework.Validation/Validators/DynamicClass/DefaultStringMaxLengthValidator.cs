using System;
using System.Reflection;

using Framework.Core;
using Framework.Restriction;

namespace Framework.Validation
{
    public class DefaultStringMaxLengthValidator : IManyPropertyDynamicClassValidator
    {
        public IPropertyValidator GetValidator(PropertyInfo property, IDynamicSource extendedValidationData)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            var availableValues = extendedValidationData.GetValue<IAvailableValues>(true);

            if (property.PropertyType != typeof(string) || property.HasAttribute<MaxLengthAttribute>()
                                                        || property.HasAttribute<MaxLengthValidatorAttribute>())
            {
                return null;
            }

            return new MaxLengthValidator(availableValues.GetAvailableSize<string>());
        }


        public static DefaultStringMaxLengthValidator Value { get; } = new DefaultStringMaxLengthValidator();
    }
}
