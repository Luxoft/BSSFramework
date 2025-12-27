using CommonFramework;

using Framework.Core;

namespace Framework.Validation;

public static class ClassValidatorExtensions
{
    public static IClassValidator<TSource> TryApplyValidationData<TSource>(this IClassValidator<TSource> baseValidator, IValidationData? validationData)
    {
        return validationData.Maybe(data => baseValidator.ApplyCustomError(data.CustomError).ApplyCustomOperationContext(data.OperationContext), baseValidator);
    }

    public static IClassValidator<TSource> ApplyCustomError<TSource>(this IClassValidator<TSource> baseValidator, object? customError)
    {
        if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));

        return customError != null ? new ClassValidatorWithOverrideError<TSource>(baseValidator, customError) : baseValidator;
    }

    public static IClassValidator<TSource> ApplyCustomOperationContext<TSource>(this IClassValidator<TSource> baseValidator, int customOperationContext)
    {
        if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));

        return customOperationContext != int.MaxValue ? new ClassValidatorWithOverrideOperationContext<TSource>(baseValidator, customOperationContext) : baseValidator;
    }

    /// <summary>
    ///     Выполняет преобразование текущего валидатора к IClassValidator{TSource}/>.
    /// </summary>
    /// <typeparam name="TSource">>Параметр результирующего типа.</typeparam>
    /// <param name="baseClassValidator">Экземпляр текущего валидатора.</param>
    /// <returns>Преобразованный валидатор.</returns>
    /// <exception cref="ArgumentNullException">Аргумент <paramref name="baseClassValidator" /> равен null.</exception>
    public static IClassValidator<TSource> TryUnbox<TSource>(this IClassValidator baseClassValidator)
    {
        if (baseClassValidator == null) throw new ArgumentNullException(nameof(baseClassValidator));

        if (!(baseClassValidator is IClassValidator<TSource>))
        {
            var args = baseClassValidator.GetType().GetInterfaceImplementationArguments(typeof(IClassValidator<>));

            if (args != null)
            {
                var validatorSourceType = args[0];

                if (validatorSourceType.IsAssignableFrom(typeof(TSource)))
                {
                    var unboxFunc = new Func<IClassValidator<object>, IClassValidator<TSource>>(Unbox<TSource, object>)
                            .CreateGenericMethod(typeof(TSource), validatorSourceType);

                    return unboxFunc.Invoke<IClassValidator<TSource>>(null, baseClassValidator);
                }
            }
        }

        return (IClassValidator<TSource>)baseClassValidator;
    }

    /// <summary>
    ///     Выполняет преобразование текущего валидатора к IClassValidator{TExpectedSource}/>.
    /// </summary>
    /// <typeparam name="TExpectedSource">Параметр результирующего типа.</typeparam>
    /// <typeparam name="TBaseSource">Параметр исходного типа.</typeparam>
    /// <param name="baseValidator">Экземпляр текущего валидатора.</param>
    /// <returns>Преобразованный валидатор.</returns>
    /// <exception cref="ArgumentNullException">Аргумент <paramref name="baseValidator" /> равен null.</exception>
    public static IClassValidator<TExpectedSource> Unbox<TExpectedSource, TBaseSource>(
            this IClassValidator<TBaseSource> baseValidator)
            where TExpectedSource : TBaseSource
    {
        if (baseValidator == null)
        {
            throw new ArgumentNullException(nameof(baseValidator));
        }

        return new UnboxedClassValidator<TExpectedSource, TBaseSource>(baseValidator);
    }

    private class ClassValidatorWithOverrideError<TSource> : IClassValidator<TSource>
    {
        private readonly IClassValidator<TSource> baseValidator;

        private readonly object customError;


        public ClassValidatorWithOverrideError(IClassValidator<TSource> baseValidator, object customError)
        {
            if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));
            if (customError == null) throw new ArgumentNullException(nameof(customError));

            this.baseValidator = baseValidator;
            this.customError = customError;
        }


        public ValidationResult GetValidationResult(IClassValidationContext<TSource> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var baseResult = this.baseValidator.GetValidationResult(context);

            return baseResult.HasErrors ? ValidationResult.CreateError(this.customError) : baseResult;
        }
    }

    private class ClassValidatorWithOverrideOperationContext<TSource> : IClassValidator<TSource>
    {
        private readonly IClassValidator<TSource> baseValidator;

        private readonly int customOperationContext;


        public ClassValidatorWithOverrideOperationContext(IClassValidator<TSource> baseValidator, int customOperationContext)
        {
            if (baseValidator == null) throw new ArgumentNullException(nameof(baseValidator));

            this.baseValidator = baseValidator;
            this.customOperationContext = customOperationContext;
        }

        public ValidationResult GetValidationResult(IClassValidationContext<TSource> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            return context.OperationContext.IsIntersected(this.customOperationContext)
                           ? this.baseValidator.GetValidationResult(context)
                           : ValidationResult.Success;
        }
    }

    private class UnboxedClassValidator<TExpectedSource, TBaseSource> : IClassValidator<TExpectedSource>
            where TExpectedSource : TBaseSource
    {
        private readonly IClassValidator<TBaseSource> baseClassValidator;


        public UnboxedClassValidator(IClassValidator<TBaseSource> baseClassValidator)
        {
            if (baseClassValidator == null) throw new ArgumentNullException(nameof(baseClassValidator));

            this.baseClassValidator = baseClassValidator;
        }


        public ValidationResult GetValidationResult(IClassValidationContext<TExpectedSource> context)
        {
            return this.baseClassValidator.GetValidationResult(context.Box<TExpectedSource, TBaseSource>());
        }
    }
}
