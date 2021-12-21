using System;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    public class DomainTypeValidatorAttribute : PropertyValidatorAttribute
    {
        public override IPropertyValidator CreateValidator()
        {
            return new DomainTypeValidator();
        }
    }


    public class DomainTypeValidator : IDynamicPropertyValidator
    {
        public IPropertyValidator GetValidator(PropertyInfo property, IDynamicSource extendedValidationData)
        {
            return (IPropertyValidator)Activator.CreateInstance(typeof(DomainTypeValidator<,>).MakeGenericType(property.ReflectedType, property.PropertyType));
        }
    }


    public class DomainTypeValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>

        where TSource : class, ISubscriptionElement
        where TProperty : class, IDomainTypeElement<DomainType>
    {
        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));


            var errorMessageRequest = from sourceDomainType in validationContext.Source.Subscription.DomainType.ToMaybe()

                                      from value in validationContext.Value.ToMaybe()

                                      from targetDomainType in value.DomainType.ToMaybe()

                                      where sourceDomainType != targetDomainType

                                      let diffName = sourceDomainType.Name != targetDomainType.Name

                                      select $"Can't apply {validationContext.Value.ToFormattedString(validationContext.GetPropertyTypeName())} [DomainType: \"{targetDomainType.Name}\"] to property {validationContext.GetPropertyName()} of {validationContext.Source.ToFormattedString(validationContext.GetSourceTypeName())} [DomainType \"{sourceDomainType.Name}\"]";


            return ValidationResult.FromMaybe(errorMessageRequest);
        }
    }
}