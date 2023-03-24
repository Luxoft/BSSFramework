using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Validation;

public static class ValidationResultExtensions
{
    public static ValidationResult Sum(this IEnumerable<ValidationResult> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Aggregate(ValidationResult.Success, (v1, v2) => v1 + v2);
    }

    public static ValidationResult Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, ValidationResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Select(selector).Sum();
    }


    public static ValidationResult ToValidationResult<TSource, TResult>(this MergeResult<TSource, TResult> mergeResult, Func<IList<TResult>, ValidationResult> addedItemsValidateFunc, Func<IList<TSource>, ValidationResult> removedItemsValidateFunc)
    {
        return addedItemsValidateFunc(mergeResult.AddingItems) + removedItemsValidateFunc(mergeResult.RemovingItems);
    }

    public static ValidationResult ToValidationResult<T>(this Framework.Core.ITryResult<T> tryResult)
    {
        return tryResult.Match(_ => ValidationResult.Success, ValidationResult.CreateError);
    }



    internal static ValidationResult Apply(this ValidationResult validationResult, ICustomErrorData errorData)
    {
        if (validationResult == null) throw new ArgumentNullException(nameof(validationResult));
        if (errorData == null) throw new ArgumentNullException(nameof(errorData));

        return validationResult.HasErrors && errorData.CustomError != null
                       ? ValidationResult.CreateError(errorData.CustomError)
                       : validationResult;
    }
}
