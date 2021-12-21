using System;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Validation
{
    public class RequiredValidator : IPropertyValidator<object, object>, IDynamicPropertyValidator
    {
        private readonly RequiredMode _mode;


        public RequiredValidator(RequiredMode mode)
        {
            this._mode = mode;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<object, object> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var property = context.Map.Property;

            this._mode.ValidateAppliedType(property.PropertyType);

            return new Func<IPropertyValidationContext<object, object>, ValidationResult>(this.GetValidationResult<object, object>)
                  .CreateGenericMethod(property.ReflectedType, property.PropertyType)
                  .Invoke<ValidationResult>(this, context);
        }

        private ValidationResult GetValidationResult<TSource, TProperty>(IPropertyValidationContext<object, object> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return new RequiredValidator<TSource, TProperty>(this._mode).GetValidationResult(context.Cast(v => (TSource)v, v => (TProperty)v));
        }


        public IPropertyValidator GetValidator(PropertyInfo property, IDynamicSource extendedValidationData)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            this._mode.ValidateAppliedType(property.PropertyType);

            return (IPropertyValidator)typeof(RequiredValidator<,>).MakeGenericType(property.ReflectedType, property.PropertyType)
                                                                   .GetConstructor(new[] {typeof(RequiredMode)})
                                                                   .Invoke(new object[] { this._mode });
        }


        public static RequiredValidator Default { get; } = new RequiredValidator(RequiredMode.Default);
    }

    public class RequiredValidator<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
    {
        private readonly RequiredMode _mode;


        public RequiredValidator(RequiredMode mode)
        {
            this._mode = mode;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> context)
        {
            return ValidationResult.FromCondition(this.IsValid(context), () =>
            {
                var name = (context.GetSource() as IVisualIdentityObject).Maybe(x => x.Name);

                if (context.Value is Period && this._mode == RequiredMode.ClosedPeriodEndDate)
                {
                    var value = (Period)(object)context.Value;

                    if (value.EndDate == null)
                    {
                        return $"The End Date for field {context.GetPropertyName()} of type {context.GetSourceTypeName()}{name.Maybe(x => $" ['{x}']")} must be initialized";
                    }
                }

                return $"The field {context.GetPropertyName()} of type {context.GetSourceTypeName()}{name.Maybe(x => $" ['{x}']")} must be initialized";
            });
        }

        protected virtual bool IsValid(IPropertyValidationContext<TSource, TProperty> context)
        {
            return RequiredHelper.IsValid(context.Value, this._mode);
        }


        public static RequiredValidator<TSource, TProperty> Default { get; } = new RequiredValidator<TSource, TProperty>(RequiredMode.Default);
    }
}