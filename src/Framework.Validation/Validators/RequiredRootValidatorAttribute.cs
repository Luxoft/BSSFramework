using System.Reflection;

using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Validation;

public class RequiredRootValidatorAttribute : PropertyValidatorAttribute
{
    public override IPropertyValidator CreateValidator()
    {
        return new RequiredRootValidator();
    }

    private class RequiredRootValidator : IDynamicPropertyValidator
    {
        public IPropertyValidator GetValidator(PropertyInfo propertyInfo, IServiceProvider serviceProvider)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var sourceType = propertyInfo.ReflectedType;

            if (sourceType.IsClass && typeof(IParentSource<>).MakeGenericType(sourceType).IsAssignableFrom(sourceType))
            {
                var validatorType = typeof(InternalRequiredRootValidator<,>).MakeGenericType(sourceType, propertyInfo.PropertyType);

                return (IPropertyValidator)Activator.CreateInstance(validatorType);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(propertyInfo));
            }
        }

        private class InternalRequiredRootValidator<TSource, TProperty> : RequiredValidator<TSource, TProperty>
                where TSource : class, IParentSource<TSource>
        {
            public InternalRequiredRootValidator()
                    : base(RequiredMode.Default)
            {
            }

            protected override bool IsValid(IPropertyValidationContext<TSource, TProperty> context)
            {
                if (context == null) throw new ArgumentNullException(nameof(context));

                return context.Source.Parent != null || base.IsValid(context);
            }
        }
    }
}
