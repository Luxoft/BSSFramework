using Anch.Core;

using Framework.Core;
using Framework.Validation.Extensions;

// ReSharper disable once CheckNamespace
namespace Framework.Validation.Validators;

public static class ClassValidatorExtensions
{
    public static IClassValidator<TSource> TryApplyValidationData<TSource>(this IClassValidator<TSource> baseValidator, IValidationData? validationData) => validationData.Maybe(data => baseValidator.ApplyCustomError(data.CustomError).ApplyCustomOperationContext(data.OperationContext), baseValidator);

    public static IClassValidator<TSource> ApplyCustomError<TSource>(this IClassValidator<TSource> baseValidator, object? customError)
    {
        if (baseValidator is null) throw new ArgumentNullException(nameof(baseValidator));

        return customError is not null ? new ClassValidatorWithOverrideError<TSource>(baseValidator, customError) : baseValidator;
    }

    public static IClassValidator<TSource> ApplyCustomOperationContext<TSource>(this IClassValidator<TSource> baseValidator, int customOperationContext)
    {
        if (baseValidator is null) throw new ArgumentNullException(nameof(baseValidator));

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
        if (baseClassValidator is null) throw new ArgumentNullException(nameof(baseClassValidator));

        if (!(baseClassValidator is IClassValidator<TSource>))
        {
            var args = baseClassValidator.GetType().GetInterfaceImplementationArguments(typeof(IClassValidator<>));

            if (args is not null)
            {
                var validatorSourceType = args[0];

                if (validatorSourceType.IsAssignableFrom(typeof(TSource)))
                {
                    var unboxFunc = new Func<IClassValidator<object>, IClassValidator<object>>(Unbox<object, object>)
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
        if (baseValidator is null)
        {
            throw new ArgumentNullException(nameof(baseValidator));
        }

        return new UnboxedClassValidator<TExpectedSource, TBaseSource>(baseValidator);
    }

    private class ClassValidatorWithOverrideError<TSource>(IClassValidator<TSource> baseValidator, object customError) : IClassValidator<TSource>
    {
        public ValidationResult GetValidationResult(IClassValidationContext<TSource> context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            var baseResult = baseValidator.GetValidationResult(context);

            return baseResult.HasErrors ? ValidationResult.CreateError(customError) : baseResult;
        }
    }

    private class ClassValidatorWithOverrideOperationContext<TSource>(IClassValidator<TSource> baseValidator, int customOperationContext)
        : IClassValidator<TSource>
    {
        private readonly IClassValidator<TSource> baseValidator = baseValidator ?? throw new ArgumentNullException(nameof(baseValidator));

        public ValidationResult GetValidationResult(IClassValidationContext<TSource> context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            return context.OperationContext.IsIntersected(customOperationContext)
                           ? this.baseValidator.GetValidationResult(context)
                           : ValidationResult.Success;
        }
    }

    private class UnboxedClassValidator<TExpectedSource, TBaseSource>(IClassValidator<TBaseSource> baseClassValidator) : IClassValidator<TExpectedSource>
        where TExpectedSource : TBaseSource
    {
        private readonly IClassValidator<TBaseSource> baseClassValidator = baseClassValidator ?? throw new ArgumentNullException(nameof(baseClassValidator));

        public ValidationResult GetValidationResult(IClassValidationContext<TExpectedSource> context) => this.baseClassValidator.GetValidationResult(context.Box<TExpectedSource, TBaseSource>());
    }
}

