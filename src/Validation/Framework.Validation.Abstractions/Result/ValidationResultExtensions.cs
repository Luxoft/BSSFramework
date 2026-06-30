using Anch.Core;

using Framework.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Validation;

public static class ValidationResultExtensions
{
    public static ValidationResult Sum(this IEnumerable<ValidationResult> source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Aggregate(ValidationResult.Success, (v1, v2) => v1 + v2);
    }

    public static ValidationResult Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, ValidationResult> selector)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        return source.Select(selector).Sum();
    }


    public static ValidationResult ToValidationResult<TSource, TResult>(this MergeResult<TSource, TResult> mergeResult, Func<IEnumerable<TResult>, ValidationResult> addedItemsValidateFunc, Func<IEnumerable<TSource>, ValidationResult> removedItemsValidateFunc) => addedItemsValidateFunc(mergeResult.AddingItems) + removedItemsValidateFunc(mergeResult.RemovingItems);

    public static ValidationResult ToValidationResult<T>(this ITryResult<T> tryResult) => tryResult.Match(_ => ValidationResult.Success, ValidationResult.CreateError);

    internal static ValidationResult Apply(this ValidationResult validationResult, ICustomErrorData errorData)
    {
        if (validationResult is null) throw new ArgumentNullException(nameof(validationResult));
        if (errorData is null) throw new ArgumentNullException(nameof(errorData));

        return validationResult.HasErrors && errorData.CustomError is not null
                       ? ValidationResult.CreateError(errorData.CustomError)
                       : validationResult;
    }
}

