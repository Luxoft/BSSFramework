using System.Reflection;

using CommonFramework;
using CommonFramework.Maybe;

using Framework.Core;

namespace Framework.QueryLanguage;

public static class StandartExpressionExtensions
{
    internal static System.Linq.Expressions.Expression TryNormalize(this System.Linq.Expressions.Expression baseExpression, Type leftType, Type rightType)
    {
        if (baseExpression == null) throw new ArgumentNullException(nameof(baseExpression));
        if (leftType == null) throw new ArgumentNullException(nameof(leftType));
        if (rightType == null) throw new ArgumentNullException(nameof(rightType));

        var tryNullableType = new[] { leftType, rightType }.FirstOrDefault(type => type.IsNullable()) ?? TryGetNullableFromNull(leftType, rightType);

        var tryEnumType = new[] { leftType, rightType }.FirstOrDefault(type => type.IsEnum);

        var request = from nullableType in tryNullableType.ToMaybe()

                      select LiftToNullable(baseExpression, nullableType);

        return request.Or(() => from enumType in tryEnumType.ToMaybe()

                                select TryConvertToEnumExpression(baseExpression, enumType))


                      .Or(() => from superType in leftType.GetSuperSet(rightType, false).ToMaybe()

                                where baseExpression.Type != superType

                                select UpToSuperType(baseExpression, superType))

                      .GetValueOrDefault(baseExpression);
    }

    private static Type TryGetNullableFromNull(Type leftType, Type rightType)
    {
        if (leftType == null) throw new ArgumentNullException(nameof(leftType));
        if (rightType == null) throw new ArgumentNullException(nameof(rightType));

        var arr = new[] { leftType, rightType };

        return arr.Contains(typeof(object)) ? arr.Where(t => t.IsValueType).Select(t => typeof(Nullable<>).MakeGenericType(t)).SingleOrDefault() : null;
    }

    private static System.Linq.Expressions.Expression UpToSuperType(System.Linq.Expressions.Expression baseExpression, Type superType)
    {
        if (baseExpression == null) throw new ArgumentNullException(nameof(baseExpression));
        if (superType == null) throw new ArgumentNullException(nameof(superType));

        return (from constValue in baseExpression.GetDeepMemberConstValue()

                let converterValue = Convert.ChangeType(constValue, superType, null)

                select System.Linq.Expressions.Expression.Constant(converterValue))

                .GetValueOrDefault(() => (System.Linq.Expressions.Expression)System.Linq.Expressions.Expression.Convert(baseExpression, superType));



    }


    public static System.Linq.Expressions.Expression TryConvertToEnumExpression(this System.Linq.Expressions.Expression baseExpression, Type enumType)
    {
        if (baseExpression == null) throw new ArgumentNullException(nameof(baseExpression));
        if (enumType == null) throw new ArgumentNullException(nameof(enumType));

        var request = from value in baseExpression.GetDeepMemberConstValue()

                      where value != null && enumType.IsEnum && value.GetType() != enumType

                      from enumValue in TryConvertToEnum(value, enumType)

                      select System.Linq.Expressions.Expression.Constant(enumValue);


        return request.GetValueOrDefault(baseExpression);

    }

    private static Maybe<object> TryConvertToEnum(object value, Type enumType)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));
        if (enumType == null) throw new ArgumentNullException(nameof(enumType));

        return (from strValue in (value as string).ToMaybe()

                select Enum.Parse(enumType, strValue, true))

                .Or(() => from underType in Convert.ChangeType(value, Enum.GetUnderlyingType(enumType), null).ToMaybe()

                          select Enum.ToObject(enumType, underType));
    }

    private static System.Linq.Expressions.Expression LiftToNullable(System.Linq.Expressions.Expression expression, Type expectedNullableType)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        if (expression.Type.IsNullable())
        {
            return expression;
        }

        if (expression.Type.IsValueType && typeof(Nullable<>).MakeGenericType(expression.Type) == expectedNullableType)
        {
            return System.Linq.Expressions.Expression.Convert(expression, expectedNullableType);
        }

        return expression.GetDeepMemberConstValue().Select(value =>
                                                           {
                                                               if (value == null)
                                                               {
                                                                   return System.Linq.Expressions.Expression.Constant(null, expectedNullableType);
                                                               }
                                                               else
                                                               {
                                                                   var expectedNullableElementType = expectedNullableType.GetNullableElementType();

                                                                   if (value.GetType() != expectedNullableElementType && expectedNullableElementType.IsEnum)
                                                                   {
                                                                       return LiftToNullable(TryConvertToEnumExpression(expression, expectedNullableElementType), expectedNullableType);
                                                                   }
                                                                   else
                                                                   {
                                                                       var liftedValue = CreateNullableConstantMethod.MakeGenericMethod(expression.Type, expectedNullableElementType).Invoke(null, new[] { value });

                                                                       return System.Linq.Expressions.Expression.Constant(liftedValue, expectedNullableType);
                                                                   }
                                                               }
                                                           }).GetValue(() => new Exception("fail"));
    }


    private static readonly MethodInfo CreateNullableConstantMethod = new Func<int, int?>(ToNullableValue<int, int>).Method.GetGenericMethodDefinition();

    internal static TExpectedValue? ToNullableValue<TConstValue, TExpectedValue>(TConstValue constValue)
            where TConstValue : struct
            where TExpectedValue : struct
    {
        return (TExpectedValue)(object)Convert.ChangeType(constValue, typeof(TExpectedValue), null);
    }
}
