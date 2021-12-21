using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Restriction;

namespace Framework.Validation
{
    public class RequiredGroupValidator : DynamicClassValidator
    {
        private readonly RequiredGroupValidatorMode _mode;

        private readonly string _groupKey;


        public RequiredGroupValidator(RequiredGroupValidatorMode mode, string groupKey)
        {
            if (!Enum.IsDefined(typeof (RequiredGroupValidatorMode), mode)) throw new ArgumentOutOfRangeException(nameof(mode));

            this._mode = mode;
            this._groupKey = groupKey;
        }


        protected override IClassValidator GetValidator<TSource>(IDynamicSource extendedValidationData)
        {
            if (extendedValidationData == null) throw new ArgumentNullException(nameof(extendedValidationData));

            var uniProperties = typeof(TSource).GetUniqueElementPropeties(this._groupKey, true);

            var uniqueElementString = uniProperties.GetUniqueElementString(false);

            var propertyValidators = uniProperties.ToDictionary(property => property.Name, GetPropertyRequiredValidator<TSource>);

            return new RequiredGroupValidator<TSource>(this._mode, propertyValidators, uniqueElementString);
        }

        private static Func<TSource, bool> GetPropertyRequiredValidator<TSource>(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            return new Func<Expression<Func<TSource, object>>, Func<TSource, bool>>(GetPropertyRequiredValidator)
                  .CreateGenericMethod(typeof(TSource), property.PropertyType)
                  .Invoke<Func<TSource, bool>>(null, property.ToLambdaExpression());
        }

        private static Func<TSource, bool> GetPropertyRequiredValidator<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyExpr)
        {
            if (propertyExpr == null) throw new ArgumentNullException(nameof(propertyExpr));

            var isValidExpr = from propertyValue in propertyExpr

                              select RequiredHelper.IsValid(propertyValue, RequiredMode.Default);

            return isValidExpr.Compile(LambdaCompileCacheContainer.Get<TSource, TProperty>());
        }


        private static readonly LambdaCompileCacheContainer LambdaCompileCacheContainer = new LambdaCompileCacheContainer();
    }

    public class RequiredGroupValidator<TSource> : IClassValidator<TSource>
    {
        private readonly RequiredGroupValidatorMode _mode;

        private readonly Dictionary<string, Func<TSource, bool>> _propertyValidators;

        private readonly string _uniqueElementString;


        public RequiredGroupValidator(RequiredGroupValidatorMode mode, Dictionary<string, Func<TSource, bool>> propertyValidators, string uniqueElementString)
        {
            this._mode = mode;
            this._propertyValidators = propertyValidators;
            this._uniqueElementString = uniqueElementString;
        }


        public ValidationResult GetValidationResult(IClassValidationContext<TSource> validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            var validState = this._propertyValidators.ChangeValue(f => f(validationContext.Source))
                                                     .Where(pair => pair.Value).ToDictionary();


            if (this._mode == RequiredGroupValidatorMode.OneOrNothing)
            {
                if (validState.Count > 1)
                {
                    return ValidationResult.CreateError("More one of properties ({0}) initialized", this._uniqueElementString);
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else if (this._mode == RequiredGroupValidatorMode.AllOrNothing)
            {
                if (validState.IsEmpty() || validState.Count == this._propertyValidators.Count)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return ValidationResult.CreateError("All or no one of properties ({0}) must initialized", this._uniqueElementString);
                }
            }
            else if (!validState.Any())
            {
                return ValidationResult.CreateError("No one of properties ({0}) not initialized", this._uniqueElementString);
            }
            else if (this._mode == RequiredGroupValidatorMode.One && validState.Count > 1)
            {
                return ValidationResult.CreateError("More one of properties ({0}) initialized", this._uniqueElementString);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}