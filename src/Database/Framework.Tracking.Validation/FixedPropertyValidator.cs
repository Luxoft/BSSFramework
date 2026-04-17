using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Validation;
using Framework.Validation.Validators;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Tracking.Validation;

/// <summary>
/// Валидатор проверки неизменяемости свойства
/// </summary>
public class FixedPropertyValidator : IDynamicPropertyValidator
{
    public IPropertyValidator GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        var persistentDomainObjectBaseTypeResolver = serviceProvider.GetRequiredService<IPersistentDomainObjectBaseTypeResolver>();

        var persistentDomainObjectBaseType = persistentDomainObjectBaseTypeResolver.Resolve(property.ReflectedType!);

        var validatorType = typeof(FixedPropertyValidator<,,>)
            .MakeGenericType(property.ReflectedType!, property.PropertyType, persistentDomainObjectBaseType);

        return serviceProvider.GetRequiredService<IServiceProxyFactory>().Create<IPropertyValidator>(validatorType, property.ToGetLambdaExpression());
    }
}

/// <summary>
/// Валидатор проверки неизменяемости свойства
/// </summary>
public class FixedPropertyValidator<TSource, TProperty, TPersistentDomainObjectBase>(Expression<Func<TSource, TProperty>> propertyPath)
    : IPropertyValidator<TSource, TProperty>
    where TSource : class, TPersistentDomainObjectBase
{
    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
    {
        var trackingService = validationContext.ServiceProvider.GetRequiredService<ITrackingService<TPersistentDomainObjectBase>>();

        return ValidationResult.FromCondition(
            trackingService.GetPersistentState(validationContext.Source) == PersistentLifeObjectState.NotPersistent
            || !trackingService.GetChanges(validationContext.Source).HasChange(propertyPath),
            () => $"{validationContext.GetPropertyName()} field in {validationContext.GetSourceTypeName()} can't be changed");
    }
}
