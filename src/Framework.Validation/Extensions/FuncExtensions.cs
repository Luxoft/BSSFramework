using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Validation;

internal static class FuncExtensions
{
    public static Func<T, ValidationResult> Or<T>(this Func<T, ValidationResult> f1, Func<T, ValidationResult> f2)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2));

        return arg =>
               {
                   var res = f1(arg);

                   if (res.HasErrors)
                   {
                       return res + f2(arg);
                   }
                   else
                   {
                       return res;
                   }
               };
    }

    public static Func<T, ValidationResult> And<T>(this Func<T, ValidationResult> f1, Func<T, ValidationResult> f2)
    {
        if (f1 == null) throw new ArgumentNullException(nameof(f1));
        if (f2 == null) throw new ArgumentNullException(nameof(f2));

        return arg => f1(arg) + f2(arg);
    }

    public static Func<T, ValidationResult> Sum<T>(this IEnumerable<Func<T, ValidationResult>> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        return items.Aggregate(ValidationResult.GetSuccessFunc<T>(), (f1, f2) => f1.And(f2));
    }

    public static Func<Validator, TSource, int, ValidationResult> WithTryEmptySource<TSource>(this Func<Validator, TSource, int, ValidationResult> baseFunc)
    {
        return (validator, source, operationContext) => source == null ? ValidationResult.Success : baseFunc(validator, source, operationContext);
    }
}
