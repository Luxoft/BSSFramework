using System;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;
using Framework.Validation;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Валидатор проверки неизменяемости свойства
/// </summary>
public class FixedPropertyValidator : IDynamicPropertyValidator
{
    public IPropertyValidator GetValidator(PropertyInfo property, IDynamicSource extendedValidationData)
    {
        var identType = property.DeclaringType.GetIdentType();

        return (IPropertyValidator)Activator.CreateInstance(typeof(FixedPropertyValidator<,,>)
                                                                    .MakeGenericType(property.ReflectedType, property.PropertyType, identType), property.ToLambdaExpression());
    }
}

/// <summary>
/// Валидатор проверки неизменяемости свойства
/// </summary>
public class FixedPropertyValidator<TSource, TProperty, TIdent> : IPropertyValidator<TSource, TProperty>
        where TSource : IIdentityObject<TIdent>
{
    private readonly Expression<Func<TSource, TProperty>> propertyPath;

    public FixedPropertyValidator(Expression<Func<TSource, TProperty>> propertyPath)
    {
        this.propertyPath = propertyPath ?? throw new ArgumentNullException(nameof(propertyPath));
    }

    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
    {
        var trackingServiceContainer = validationContext.ExtendedValidationData.GetValue<ITrackingServiceContainer<TSource>>(true);

        return ValidationResult.FromCondition(
                                              trackingServiceContainer.TrackingService.GetPersistentState(validationContext.Source) == PersistentLifeObjectState.NotPersistent
                                              || !trackingServiceContainer.TrackingService.GetChanges(validationContext.Source).HasChange(this.propertyPath),
                                              () => $"{validationContext.GetPropertyName()} field in {validationContext.GetPropertyTypeName()} can't be changed");
    }
}
