using System;
using System.Linq;

using Framework.Core;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    internal static class RuntimeExtractValueService
    {
        internal static object TryExtract(object value)
        {
            if (value == null)
            {
                return null;
            }

            var extractActions = new Func<object, Maybe<object>>[]
                                 {
                                     TryJustExtract,
                                     TryNullableExtract,
                                     TryIADFrameworkNullableExtract
                                 };

            var result = value;

            while (true)
            {
                var tryNextResult = extractActions.Select(z => z(result)).FirstOrDefault(z => z.HasValue);
                if (null == tryNextResult)
                {
                    break;
                }

                result = tryNextResult.GetValue();
            }

            return result;
        }

        private static Maybe<object> TryJustExtract(object value)
        {
            var tryJustElementType = value.GetType().GetMaybeElementType();
            if (null != tryJustElementType)
            {
                var nextValue = new Func<Just<object>, object>(TryJustExtract)
                    .CreateGenericMethod(tryJustElementType)
                    .Invoke(typeof(CellValueFuncFactory), new object[] { value });

                return Maybe.Return(nextValue);
            }

            return Maybe<object>.Nothing;
        }

        private static object TryJustExtract<TValue>(Just<TValue> value)
        {
            if (null == value)
            {
                return null;
            }

            return value.Value;
        }

        private static Maybe<object> TryNullableExtract(object value)
        {
            var tryNullableElementType = value.GetType().GetNullableElementType();
            if (null != tryNullableElementType)
            {
                var result = new Func<int?, object>(TryNullableExtract)
                    .CreateGenericMethod(tryNullableElementType)
                    .Invoke(typeof(CellValueFuncFactory), new object[] { value });

                return Maybe.Return(result);
            }

            return Maybe<object>.Nothing;
        }

        private static object TryNullableExtract<TValue>(TValue? value) where TValue : struct
        {
            if (null == value)
            {
                return null;
            }

            return value.Value;
        }

        private static Maybe<object> TryIADFrameworkNullableExtract(object value)
        {
            var tryNullableElementType = value.GetType().GetNullableElementType();
            if (null != tryNullableElementType)
            {
                var result = new Func<NullableObject<object>, object>(TryIADFrameworkNullableExtract)
                    .CreateGenericMethod(tryNullableElementType)
                    .Invoke(typeof(CellValueFuncFactory), new object[] { value });

                return Maybe.Return(result);
            }

            return Maybe<object>.Nothing;
        }

        private static object TryIADFrameworkNullableExtract<TValue>(NullableObject<TValue> value)
        {
            if (null == value)
            {
                return null;
            }

            return value.Value;
        }
    }
}