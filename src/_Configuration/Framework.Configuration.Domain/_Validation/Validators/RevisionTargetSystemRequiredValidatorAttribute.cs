using System;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    public class RevisionTargetSystemRequiredValidatorAttribute : PropertyValidatorAttribute
    {
        public override IPropertyValidator CreateValidator()
        {
            return new RevisionTargetSystemRequiredValidator();
        }
    }

    public class RevisionTargetSystemRequiredValidator : IDynamicPropertyValidator
    {
        public IPropertyValidator GetValidator(PropertyInfo property, IDynamicSource extendedValidationData)
        {
            return (IPropertyValidator)Activator.CreateInstance(typeof(RevisionTargetSystemRequiredValidator<,>).MakeGenericType(property.ReflectedType, property.PropertyType));
        }
    }

    public class RevisionTargetSystemRequiredValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>

        where TSource : class, ITargetSystemElement<TargetSystem>
    {
        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));


            return validationContext.Source.TargetSystem.Maybe(ts => ts.IsRevision)
                ? RequiredValidator<TSource, TProperty>.Default.GetValidationResult(validationContext)
                : ValidationResult.Success;
        }
    }
}