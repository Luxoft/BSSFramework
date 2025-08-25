using System.Linq.Expressions;
using CommonFramework.Maybe;

namespace Framework.Core;

public static class ExpressionExtensions
{
    public static Expression ExtractBoxingValue(this Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return expression.GetConvertOperand().GetValueOrDefault(expression);
    }

    public static Maybe<Expression> GetConvertOperand(this Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return from unaryExpression in (expression as UnaryExpression).ToMaybe()

               where expression.NodeType == ExpressionType.Convert

               select unaryExpression.Operand;
    }

    public static string ToPath(this Expression source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));


        var result = string.Empty;
        var expressionType = source.GetType();
        if (source is LambdaExpression)
        {
            var body = ((LambdaExpression)source).Body;
            return body.ToPath();
        }

        if (source is MemberExpression)
        {
            var memberExpression = (MemberExpression)source;
            var leftExpression = memberExpression.Expression;
            var rightPath = memberExpression.Member.Name;

            if (leftExpression is ConstantExpression) // TODO: rewrite to common case
            {
                var constantExpression = (ConstantExpression)leftExpression;
                var field = constantExpression.Value.GetType().GetField(rightPath);

                return field.GetValue(constantExpression.Value).ToString();
            }

            var leftPath = leftExpression.ToPath();
            return leftPath.MaybeString(z => z + "." + rightPath).IfDefaultString(rightPath);
        }

        if (source is UnaryExpression)
        {
            return ((UnaryExpression)source).Operand.ToPath();
        }

        if (source is BinaryExpression)
        {
            var binaryExpression = (BinaryExpression)source;
            return $"{binaryExpression.Left.ToPath()}{ToStringRepresentation(binaryExpression.NodeType, binaryExpression.Right)}";
        }

        if (source is ConstantExpression)
        {
            var constantExpression = (ConstantExpression)source;

            return constantExpression.Value.ToString();
        }

        if (source is MethodCallExpression)
        {
            var methodCallExpression = (MethodCallExpression)source;
            var @object = methodCallExpression.Object;
            var leftPath = @object.Maybe(z => z.ToPath(), string.Empty);
            if (string.Equals(
                    methodCallExpression.Method.Name,
                    "get_Item",
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return $"{leftPath.MaybeString(z => z)}[{string.Join(",", methodCallExpression.Arguments.Select(z => ToPath(z)))}]";
            }

            return
                $"{leftPath.MaybeString(z => z + ".")}{methodCallExpression.Method.Name}({string.Join(",", methodCallExpression.Arguments.Select(z => z.ToPath()))})";
        }

        return result;
    }

    private static string ToStringRepresentation(ExpressionType source, Expression expression)
    {
        switch (source)
        {
            case ExpressionType.ArrayIndex:
                return $"[{expression.ToPath()}]";
            default:
                throw new NotSupportedException(
                    $"Not supported expressionType format. ExpressionType:{source.ToString()}");
        }
    }

    public static Expression TryLiftToNullable(this Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        if (expression.Type.IsNullable() || !expression.Type.IsValueType)
        {
            return expression;
        }
        else
        {
            return Expression.Convert(expression, typeof(Nullable<>).MakeGenericType(expression.Type));
        }
    }
}
