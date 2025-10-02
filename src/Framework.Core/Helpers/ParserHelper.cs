using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;
using CommonFramework.ExpressionEvaluate;
using CommonFramework.Maybe;
using static CommonFramework.Maybe.Maybe;

namespace Framework.Core.Serialization;

public static class ParserHelper
{
    public static T Parse<T>(string value)
    {
        return GetParseFunc<T>()(value);
    }

    public static Maybe<T> TryParse<T>(string value)
    {
        return GetTryParseFunc<T>()(value);
    }


    public static LambdaExpression GetParseExpression(Type type, bool raiseError = true)
    {
        return new Func<bool, Expression<Func<string, object>>>(GetParseExpression<object>)
               .CreateGenericMethod(type)
               .Invoke<LambdaExpression>(null, raiseError);
    }

    public static Expression<Func<string, T>> GetParseExpression<T>(bool raiseError = true)
    {
        var expr = InternalHelper<T>.ParseExpression;

        if (expr == null && raiseError)
        {
            throw new Exception($"Parsing Func for type {typeof(T).Name} not found");
        }

        return expr;
    }

    public static Delegate GetParseFunc(Type type, bool raiseError = true)
    {
        return new Func<bool, Func<string, object>>(GetParseFunc<object>)
               .CreateGenericMethod(type)
               .Invoke<Delegate>(null, raiseError);
    }

    public static Func<string, T> GetParseFunc<T>(bool raiseError = true)
    {
        var func = InternalHelper<T>.ParseFunc;

        if (func == null && raiseError)
        {
            throw new Exception($"Parsing Func for type {typeof(T).Name} not found");
        }

        return func;
    }


    public static LambdaExpression GetTryParseExpression(Type type, bool raiseError = true)
    {
        return new Func<bool, Expression<Func<string, Maybe<object>>>>(GetTryParseExpression<object>)
               .CreateGenericMethod(type)
               .Invoke<LambdaExpression>(null, raiseError);
    }

    public static Expression<Func<string, Maybe<T>>> GetTryParseExpression<T>(bool raiseError = true)
    {
        var expr = InternalHelper<T>.TryParseExpression;

        if (expr == null && raiseError)
        {
            throw new Exception($"TryParsing Func for type {typeof(T).Name} not found");
        }

        return expr;
    }

    public static Delegate? GetTryParseFunc(Type type, bool raiseError = true)
    {
        return new Func<bool, Func<string, object>>(GetTryParseFunc<object>)
               .CreateGenericMethod(type)
               .Invoke<Delegate>(null, raiseError);
    }

    public static Func<string, Maybe<T>>? GetTryParseFunc<T>(bool raiseError = true)
    {
        var func = InternalHelper<T>.TryParseFunc;

        if (func == null && raiseError)
        {
            throw new Exception($"TryParsing Func for type {typeof(T).Name} not found");
        }

        return func;
    }

    private static class InternalHelper<T>
    {
        public static readonly Expression<Func<string, T>>? ParseExpression = GetParseExpression();

        public static readonly Func<string, T>? ParseFunc = ParseExpression?.Compile();


        public static readonly Expression<Func<string, Maybe<T>>>? TryParseExpression = GetTryParseExpression();

        public static readonly Func<string, Maybe<T>>? TryParseFunc = TryParseExpression?.Compile();



        private static Expression<Func<string, T>>? GetParseExpression()
        {
            var parameter = Expression.Parameter(typeof(string));

            return GetParseExpressionBody(parameter).Maybe(body => Expression.Lambda<Func<string, T>>(body, parameter));
        }

        private static Expression? GetParseExpressionBody(Expression parameter)
        {
            if (typeof(T) == typeof(string))
            {
                return parameter;
            }
            else if (typeof(T).IsEnum)
            {
                var parseMethod = new Func<string, TypeCode>(EnumHelper.Parse<TypeCode>).Method.GetGenericMethodDefinition();

                return Expression.Call(parseMethod, parameter);
            }
            else
            {
                return typeof(T).GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null)
                                .Maybe(parseMethod => Expression.Call(parseMethod, parameter));
            }
        }


        private static Expression<Func<string, Maybe<T>>>? GetTryParseExpression()
        {
            var parameter = Expression.Parameter(typeof(string));

            return GetTryParseExpressionBody(parameter).Maybe(body => Expression.Lambda<Func<string, Maybe<T>>>(body, parameter));
        }

        private static Expression? GetTryParseExpressionBody(Expression parameter)
        {
            if (typeof(T) == typeof(string))
            {
                return parameter.SafeWrapToMaybe();
            }
            else if (typeof(T).IsEnum)
            {
                var parseMethod = new Func<string, Maybe<TypeCode>>(EnumHelper.TryParse<TypeCode>).Method.GetGenericMethodDefinition();

                return Expression.Call(parseMethod, parameter);
            }
            else
            {
                return typeof(T).GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string), typeof(T).MakeByRefType() }, null).Maybe(tryParseMethod =>
                {
                    var tryParseDelType = typeof(TryMethod<,>).MakeGenericType(typeof(string), typeof(T));

                    var tryParseDel = (TryMethod<string, T>)Delegate.CreateDelegate(tryParseDelType, tryParseMethod);

                    var maybeDel = Maybe.OfTryMethod(tryParseDel);

                    return ExpressionHelper.Create((string arg) => maybeDel(arg)).GetBodyWithOverrideParameters(parameter);
                });
            }
        }
    }
}
