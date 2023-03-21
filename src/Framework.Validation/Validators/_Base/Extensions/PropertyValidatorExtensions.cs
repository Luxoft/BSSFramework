using System;

using Framework.Core;

namespace Framework.Validation;

public static class PropertyValidatorExtensions
{
    public static IPropertyValidator<TSource, TProperty> TryApplyValidationData<TSource, TProperty>(this IPropertyValidator<TSource, TProperty> baseValidator, IValidationData validationData)
    {
        return validationData.Maybe(data => baseValidator.ApplyCustomError(validationData.CustomError)
                                                         .ApplyCustomOperationContext(validationData.OperationContext),

                                    baseValidator);
    }


    public static IPropertyValidator<TSource, TProperty> ApplyCustomError<TSource, TProperty>(this IPropertyValidator<TSource, TProperty> baseValidator, object customError)
    {
        if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));

        return customError != null ? new PropertyValidatorWithOverrideError<TSource, TProperty>(baseValidator, customError) : baseValidator;
    }

    public static IPropertyValidator<TSource, TProperty> ApplyCustomOperationContext<TSource, TProperty>(this IPropertyValidator<TSource, TProperty> baseValidator, int customOperationContext)
    {
        if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));

        return customOperationContext != int.MaxValue ? new PropertyValidatorWithOverrideOperationContext<TSource, TProperty>(baseValidator, customOperationContext) : baseValidator;
    }




    public static IPropertyValidator<TSource, TProperty> TryUnbox<TSource, TProperty>(this IPropertyValidator basePropertyValidator)
    {
        if (basePropertyValidator == null) throw new ArgumentNullException(nameof(basePropertyValidator));

        if (!(basePropertyValidator is IPropertyValidator<TSource, TProperty>))
        {
            var args = basePropertyValidator.GetType().GetInterfaceImplementationArguments(typeof(IPropertyValidator<,>));

            if (args != null)
            {
                var validatorSourceType = args[0];
                var validatorPropertyType = args[1];

                if (validatorSourceType.IsAssignableFrom(typeof(TSource)) && validatorPropertyType.IsAssignableFrom(typeof(TProperty)))
                {
                    var unboxFunc = new Func<IPropertyValidator<object, object>, IPropertyValidator<TSource, TProperty>>(Unbox<TSource, object, TProperty, object>)
                            .CreateGenericMethod(typeof(TSource), validatorSourceType, typeof(TProperty), validatorPropertyType);

                    return (IPropertyValidator<TSource, TProperty>)unboxFunc.Invoke(null, new object[] { basePropertyValidator });
                }
            }
        }

        return (IPropertyValidator<TSource, TProperty>)basePropertyValidator;
    }

    public static IPropertyValidator<TExpectedSource, TExpectedProperty> Unbox<TExpectedSource, TBaseSource, TExpectedProperty, TBaseProperty>(this IPropertyValidator<TBaseSource, TBaseProperty> baseValidator)
            where TExpectedProperty : TBaseProperty
            where TExpectedSource : TBaseSource
    {
        if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));

        return new UnboxedPropertyValidator<TExpectedSource, TBaseSource, TExpectedProperty, TBaseProperty>(baseValidator);
    }




    private class PropertyValidatorWithOverrideError<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
    {
        private readonly IPropertyValidator<TSource, TProperty> _baseValidator;

        private readonly object _customError;

        public PropertyValidatorWithOverrideError(IPropertyValidator<TSource, TProperty> baseValidator, object customError)
        {
            if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));
            if (customError == null) throw new ArgumentNullException(nameof(customError));

            this._baseValidator = baseValidator;
            this._customError = customError;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> context)
        {
            var baseResult = this._baseValidator.GetValidationResult(context);

            return baseResult.HasErrors ? ValidationResult.CreateError(this._customError) : baseResult;
        }
    }



    private class PropertyValidatorWithOverrideOperationContext<TSource, TProperty> : IPropertyValidator<TSource, TProperty>
    {
        private readonly IPropertyValidator<TSource, TProperty> _baseValidator;

        private readonly int _customOperationContext;

        public PropertyValidatorWithOverrideOperationContext(IPropertyValidator<TSource, TProperty> baseValidator, int customOperationContext)
        {
            if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));

            this._baseValidator = baseValidator;
            this._customOperationContext = customOperationContext;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<TSource, TProperty> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.OperationContext.IsIntersected(this._customOperationContext)
                           ? this._baseValidator.GetValidationResult(context)
                           : ValidationResult.Success;
        }
    }



    private class UnboxedPropertyValidator<TExpectedSource, TBaseSource, TExpectedProperty, TBaseProperty> : IPropertyValidator<TExpectedSource, TExpectedProperty>
            where TExpectedProperty : TBaseProperty
            where TExpectedSource : TBaseSource
    {
        private readonly IPropertyValidator<TBaseSource, TBaseProperty> _basePropertyValidator;

        public UnboxedPropertyValidator(IPropertyValidator<TBaseSource, TBaseProperty> basePropertyValidator)
        {
            if (basePropertyValidator == null) throw new ArgumentNullException(nameof(basePropertyValidator));

            this._basePropertyValidator = basePropertyValidator;
        }


        public ValidationResult GetValidationResult(IPropertyValidationContext<TExpectedSource, TExpectedProperty> context)
        {
            return this._basePropertyValidator.GetValidationResult(context.Box<TExpectedSource, TBaseSource, TExpectedProperty, TBaseProperty>());
        }
    }
}
