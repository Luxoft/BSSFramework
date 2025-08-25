using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Restriction;

namespace Framework.Validation;

public static class RequiredHelper
{
    public static bool IsValid<TValue>(TValue value, RequiredMode requiredMode)
    {
        return InternalHelper<TValue>.IsValidFunc(value, requiredMode);
    }

    public static Expression<Func<TValue, RequiredMode, bool>> GetIsValidExpression<TValue>()
    {
        return InternalHelper<TValue>.IsValidExpression;
    }




    private static bool IsValidNullable<TValue>(TValue? value, RequiredMode mode)
            where TValue : struct
    {
        switch (mode)
        {
            case RequiredMode.Default:
                return value != null;

            default:
                throw mode.GetUnappliedException<TValue?>();
        }
    }

    private static bool IsValidPeriod(Period value, RequiredMode mode)
    {
        switch (mode)
        {
            case RequiredMode.Default:
                return !value.IsEmpty();

            case RequiredMode.ClosedPeriodEndDate:
                return !value.IsEmpty() && value.EndDate != null;

            default:
                throw mode.GetUnappliedException<Period>();
        }
    }

    private static bool IsValidValueType<TValue>(TValue value, RequiredMode mode)
            where TValue : struct
    {
        switch (mode)
        {
            case RequiredMode.Default:
                return !value.IsDefault();

            default:
                throw mode.GetUnappliedException<TValue>();
        }
    }


    private static bool IsValidString(string value, RequiredMode mode)
    {
        switch (mode)
        {
            case RequiredMode.Default:
                return !value.IsNullOrWhiteSpace();

            case RequiredMode.AllowEmptyString:
                return value != null;

            default:
                throw mode.GetUnappliedException<string>();
        }
    }

    private static bool IsValidClass<TValue>(TValue value, RequiredMode mode)
            where TValue : class
    {
        switch (mode)
        {
            case RequiredMode.Default:
                return value != null;

            default:
                throw mode.GetUnappliedException<string>();
        }
    }

    private static Exception GetUnappliedException<TValue>(this RequiredMode mode)
    {
        return new Exception($"{mode.ToCSharpCode()} can't be applied to type {typeof(TValue)}");
    }


    private static class InternalHelper<TValue>
    {
        public static readonly Expression<Func<TValue, RequiredMode, bool>> IsValidExpression = GetIsValidExpression();

        public static readonly Func<TValue, RequiredMode, bool> IsValidFunc = IsValidExpression.Compile();


        private static Expression<Func<TValue, RequiredMode, bool>> GetIsValidExpression()
        {
            var valueParameter = Expression.Parameter(typeof(TValue));
            var modeParameter = Expression.Parameter(typeof(RequiredMode));

            return Expression.Lambda<Func<TValue, RequiredMode, bool>>(Expression.Call(GetIsValidMethod(), valueParameter, modeParameter), valueParameter, modeParameter);
        }

        private static MethodInfo GetIsValidMethod()
        {
            var nullableType = typeof(TValue).GetNullableElementType();

            if (nullableType != null)
            {
                return new Func<Ignore?, RequiredMode, bool>(IsValidNullable).Method.GetGenericMethodDefinition().MakeGenericMethod(nullableType);
            }
            else if (typeof(TValue) == typeof(Period))
            {
                return new Func<Period, RequiredMode, bool>(IsValidPeriod).Method;
            }
            else if (typeof(TValue).IsValueType)
            {
                return new Func<Ignore, RequiredMode, bool>(IsValidValueType).Method.GetGenericMethodDefinition().MakeGenericMethod(typeof(TValue));
            }
            else if (typeof(TValue) == typeof(string))
            {
                return new Func<string, RequiredMode, bool>(IsValidString).Method;
            }
            else
            {
                return new Func<object, RequiredMode, bool>(IsValidClass).Method.GetGenericMethodDefinition().MakeGenericMethod(typeof(TValue));
            }
        }
    }
}
