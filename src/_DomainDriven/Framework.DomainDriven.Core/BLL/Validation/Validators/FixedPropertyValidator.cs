using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Валидатор проверки неизменяемости свойства
/// </summary>
public class FixedPropertyValidator : IDynamicPropertyValidator
{
    public IPropertyValidator GetValidator(PropertyInfo property, IServiceProvider serviceProvider)
    {
        var identType = property.DeclaringType.GetIdentType();

        var persistentDomainObjectBaseTypeResolver = serviceProvider.GetRequiredService<IPersistentDomainObjectBaseTypeResolver>();

        var persistentDomainObjectBaseType = persistentDomainObjectBaseTypeResolver.Resolve(property.ReflectedType);

        return (IPropertyValidator)Activator.CreateInstance(typeof(FixedPropertyValidator<,,,>)
                                                                    .MakeGenericType(property.ReflectedType, property.PropertyType, identType, persistentDomainObjectBaseType), property.ToLambdaExpression());
    }
}

/// <summary>
/// Валидатор проверки неизменяемости свойства
/// </summary>
public class FixedPropertyValidator<TSource, TProperty, TIdent, TPersistentDomainObjectBase> : IPropertyValidator<TSource, TProperty>
    where TSource : TPersistentDomainObjectBase
    where TPersistentDomainObjectBase : IIdentityObject<TIdent>
{
    private readonly Expression<Func<TSource, TProperty>> propertyPath;

    public FixedPropertyValidator(Expression<Func<TSource, TProperty>> propertyPath)
    {
        this.propertyPath = propertyPath ?? throw new ArgumentNullException(nameof(propertyPath));
    }

    public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
    {
        var trackingService = validationContext.ServiceProvider.GetRequiredService<ITrackingService<TPersistentDomainObjectBase>>();

        return ValidationResult.FromCondition(
            trackingService.GetPersistentState(validationContext.Source) == PersistentLifeObjectState.NotPersistent
            || !trackingService.GetChanges(validationContext.Source).HasChange(this.propertyPath),
            () => $"{validationContext.GetPropertyName()} field in {validationContext.GetPropertyTypeName()} can't be changed");
    }
}
