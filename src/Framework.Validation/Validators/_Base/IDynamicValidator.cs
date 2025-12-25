using System.Reflection;

using CommonFramework;

namespace Framework.Validation;

public interface IDynamicValidator<in TInput, out TElementValidator>
{
    TElementValidator GetValidator(TInput input, IServiceProvider serviceProvider);
}

public interface IDynamicPropertyValidatorBase : IDynamicValidator<PropertyInfo, IPropertyValidator>
{

}

public interface IDynamicClassValidatorBase : IDynamicValidator<Type, IClassValidator>
{

}


public static class DynamicValidatorExtensions
{
    public static IPropertyValidator? GetLastPropertyValidator(this IPropertyValidator propertyValidator, PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (propertyValidator == null) throw new ArgumentNullException(nameof(propertyValidator));
        if (property == null) throw new ArgumentNullException(nameof(property));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        if (propertyValidator is IDynamicPropertyValidatorBase dynamicPropertyValidatorBase)
        {
            return dynamicPropertyValidatorBase.GetLastPropertyValidator(property, serviceProvider);
        }
        else
        {
            return propertyValidator;
        }
    }

    public static IPropertyValidator? GetLastPropertyValidator(this IDynamicPropertyValidatorBase dynamicValidator, PropertyInfo property, IServiceProvider serviceProvider)
    {
        if (dynamicValidator == null) throw new ArgumentNullException(nameof(dynamicValidator));
        if (property == null) throw new ArgumentNullException(nameof(property));

        return dynamicValidator.GetValidator(property, serviceProvider).Maybe(propertyValidator => propertyValidator.GetLastPropertyValidator(property, serviceProvider));
    }


    public static IClassValidator? GetLastClassValidator(this IClassValidator typeValidator, Type type, IServiceProvider serviceProvider)
    {
        if (typeValidator == null) throw new ArgumentNullException(nameof(typeValidator));
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        if (typeValidator is IDynamicClassValidatorBase dynamicClassValidatorBase)
        {
            return dynamicClassValidatorBase.GetLastClassValidator(type, serviceProvider);
        }
        else
        {
            return typeValidator;
        }
    }

    public static IClassValidator? GetLastClassValidator(this IDynamicClassValidatorBase dynamicValidator, Type type, IServiceProvider serviceProvider)
    {
        if (dynamicValidator == null) throw new ArgumentNullException(nameof(dynamicValidator));
        if (type == null) throw new ArgumentNullException(nameof(type));

        return dynamicValidator.GetValidator(type, serviceProvider).Maybe(typeValidator => typeValidator.GetLastClassValidator(type, serviceProvider));
    }
}
