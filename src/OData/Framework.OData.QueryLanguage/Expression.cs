using System.Reflection;

using CommonFramework;
using CommonFramework.Maybe;

using Framework.Core;
using Framework.OData.QueryLanguage.Constant;
using Framework.OData.QueryLanguage.Constant.Base;

using SExpressions = System.Linq.Expressions;

namespace Framework.OData.QueryLanguage;

public abstract record Expression
{
    public static LambdaExpression Create<TDelegate>(SExpressions.Expression<TDelegate> standardExpression) =>
        (LambdaExpression)Create((SExpressions.Expression)standardExpression);

    internal static Expression Create(SExpressions.Expression baseExpression) =>
        CreateInternal(baseExpression.ExtractBoxingValue());

    internal static Expression CreateInternal(SExpressions.Expression baseExpression) =>

        (from expression in (baseExpression as SExpressions.LambdaExpression).ToMaybe()

         select new LambdaExpression(expression))


        .Or(() => from expression in baseExpression.GetDeepMemberConstExpression()

                  select (Expression)CreateConstant(expression.Type, expression.Value))


        .Or(() => from expression in (baseExpression as SExpressions.MemberExpression).ToMaybe()

                  where expression.Member is PropertyInfo || expression.Member is FieldInfo

                  select (Expression)new PropertyExpression(Create(expression.Expression!), expression.Member.Name))


        .Or(() => from expression in (baseExpression as SExpressions.BinaryExpression).ToMaybe()

                  from left in expression.Left.GetConvertOperand()

                  where left.Type.IsEnum

                  from enumUnderValue in expression.Right.GetDeepMemberConstValue()

                  let enumValue = Enum.ToObject(left.Type, enumUnderValue)

                  let enumValueConst = SExpressions.Expression.Constant(enumValue)

                  select CreateInternal(SExpressions.Expression.MakeBinary(expression.NodeType, left, enumValueConst)))


        .Or(() => from expression in (baseExpression as SExpressions.BinaryExpression).ToMaybe()

                  from right in expression.Right.GetConvertOperand()

                  where right.Type.IsEnum

                  from enumUnderValue in expression.Left.GetDeepMemberConstValue()

                  let enumValue = Enum.ToObject(right.Type, enumUnderValue)

                  let enumValueConst = SExpressions.Expression.Constant(enumValue)

                  select CreateInternal(SExpressions.Expression.MakeBinary(expression.NodeType, enumValueConst, right)))


        .Or(() => from expression in (baseExpression as SExpressions.BinaryExpression).ToMaybe()

                  select (Expression)new BinaryExpression(expression))


        .Or(() => from expression in (baseExpression as SExpressions.ParameterExpression).ToMaybe()

                  select (Expression)new ParameterExpression(expression.Name!))

        .Or(() => from expression in (baseExpression as SExpressions.MethodCallExpression).ToMaybe()

                  from methodType in expression.Method.GetMethodType().ToMaybe()

                  select (Expression)expression.GetChildren()
                                               .Select(Create)
                                               .GetByFirst((head, tail) => new MethodExpression(head, methodType, [.. tail])))

        .GetValue(() => new NotImplementedException());

    internal static ConstantExpression CreateConstant(Type constType, object? constValue)
    {
        if (constType == null) throw new ArgumentNullException(nameof(constType));

        if (constValue == null)
        {
            return NullConstantExpression.Value;
        }

        if (constType == typeof(string))
        {
            return new StringConstantExpression((string)constValue);
        }

        if (constType == typeof(bool))
        {
            return new BooleanConstantExpression((bool)constValue);
        }

        if (constType == typeof(int))
        {
            return new Int32ConstantExpression((int)constValue);
        }

        if (constType == typeof(long))
        {
            return new Int64ConstantExpression((long)constValue);
        }

        if (constType == typeof(DateTime))
        {
            return new DateTimeConstantExpression((DateTime)constValue);
        }

        if (constType == typeof(Guid))
        {
            return new GuidConstantExpression((Guid)constValue);
        }

        if (constType.IsEnum)
        {
            return new EnumConstantExpression((Enum)constValue);
        }

        if (constType.IsNullable())
        {
            return CreateNullableConstantMethod.MakeGenericMethod(constType.GetGenericArguments().Single())
                                               .Invoke<ConstantExpression>(null, [constValue]);

        }

        throw new NotImplementedException();
    }

    private static readonly MethodInfo CreateNullableConstantMethod = new Func<int?, ConstantExpression>(CreateNullableConstant).Method.GetGenericMethodDefinition();

    internal static ConstantExpression CreateNullableConstant<TConstValue>(TConstValue? constValue)
        where TConstValue : struct =>
        constValue.ToMaybe().Select(v => CreateConstant(typeof(TConstValue), v)).GetValueOrDefault((ConstantExpression)NullConstantExpression.Value);

}
