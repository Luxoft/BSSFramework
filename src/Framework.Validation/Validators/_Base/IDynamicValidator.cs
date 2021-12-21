using System;
using System.Reflection;

using Framework.Core;

namespace Framework.Validation
{
    public interface IDynamicValidator<in TInput, out TElementValidator>
    {
        TElementValidator GetValidator(TInput input, IDynamicSource extendedValidationData);
    }

    public interface IDynamicPropertyValidatorBase : IDynamicValidator<PropertyInfo, IPropertyValidator>
    {

    }

    public interface IDynamicClassValidatorBase : IDynamicValidator<Type, IClassValidator>
    {

    }


    public static class DynamicValidatorExtensions
    {
        public static IPropertyValidator GetLastPropertyValidator(this IPropertyValidator propertyValidator, PropertyInfo property, IDynamicSource extendedValidationData)
        {
            if (propertyValidator == null) throw new ArgumentNullException(nameof(propertyValidator));
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            if (propertyValidator is IDynamicPropertyValidatorBase)
            {
                return (propertyValidator as IDynamicPropertyValidatorBase).GetLastPropertyValidator(property, extendedValidationData);
            }
            else
            {
                return propertyValidator;
            }
        }

        public static IPropertyValidator GetLastPropertyValidator(this IDynamicPropertyValidatorBase dynamicValidator, PropertyInfo property, IDynamicSource extendedValidationData)
        {
            if (dynamicValidator == null) throw new ArgumentNullException(nameof(dynamicValidator));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return dynamicValidator.GetValidator(property, extendedValidationData).Maybe(propertyValidator => propertyValidator.GetLastPropertyValidator(property, extendedValidationData));
        }


        public static IClassValidator GetLastClassValidator(this IClassValidator typeValidator, Type type, IDynamicSource extendedValidationData)
        {
            if (typeValidator == null) throw new ArgumentNullException(nameof(typeValidator));
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            if (typeValidator is IDynamicClassValidatorBase)
            {
                return (typeValidator as IDynamicClassValidatorBase).GetLastClassValidator(type, extendedValidationData);
            }
            else
            {
                return typeValidator;
            }
        }

        public static IClassValidator GetLastClassValidator(this IDynamicClassValidatorBase dynamicValidator, Type type, IDynamicSource extendedValidationData)
        {
            if (dynamicValidator == null) throw new ArgumentNullException(nameof(dynamicValidator));
            if (type == null) throw new ArgumentNullException(nameof(type));

            return dynamicValidator.GetValidator(type, extendedValidationData).Maybe(typeValidator => typeValidator.GetLastClassValidator(type, extendedValidationData));
        }
    }
}