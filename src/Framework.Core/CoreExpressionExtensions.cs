using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;
using CommonFramework.Maybe;

namespace Framework.Core;

public static class CoreExpressionExtensions
{
    /// <summary>
    /// Получение имени мембера (поле или свойство)
    /// </summary>
    /// <typeparam name="TFunc"></typeparam>
    /// <param name="expr">Выражение</param>
    /// <returns></returns>
    public static string GetInstanceMemberName<TFunc>(this Expression<TFunc> expr)
    {
        if (expr == null) throw new ArgumentNullException(nameof(expr));

        var request = from memberExpr in (expr.Body as MemberExpression).ToMaybe()

                      let member = memberExpr.Member

                      where (member is PropertyInfo || member is FieldInfo)

                            && memberExpr.Expression != null

                      select member.Name;

        return request.GetValue(() => new Exception($"invalid expression: {expr}"));
    }

    public static IEnumerable<PropertyInfo> GetReverseProperties(this LambdaExpression source)
    {
        var parameter = source.Parameters.Single();

        for (var state = source.Body; state != parameter;)
        {
            var memberExpr = (MemberExpression)state;

            yield return (PropertyInfo)memberExpr.Member;

            state = memberExpr.Expression;
        }
    }

    public static PropertyPath ToPropertyPath(this LambdaExpression source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetReverseProperties().Reverse().ToPropertyPath();
    }

    /// <summary>
    /// Получение имени мембера (поле или свойство)
    /// </summary>
    /// /// <typeparam name="T"></typeparam>
    /// <param name="expr">Выражение</param>
    /// <returns></returns>
    public static string GetStaticMemberName<T>(this Expression<Func<T>> expr)
    {
        if (expr == null) throw new ArgumentNullException(nameof(expr));

        var request = from memberExpr in (expr.Body as MemberExpression).ToMaybe()

                      let member = memberExpr.Member

                      where (member is PropertyInfo || member is FieldInfo)

                            && memberExpr.Expression == null

                      select member.Name;

        return request.GetValue(() => new Exception($"invalid expression: {expr}"));
    }

    public static Node<Expression> ToNode(this Expression expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        var visitor = new NodeExpressionVisitor(expression);

        visitor.Visit(expression);

        return visitor.ToNode();
    }


    public static Expression<Func<TTo, TRetType>> Covariance<TTo, TFrom, TRetType>(this Expression<Func<TFrom, TRetType>> source)
        where TTo : TFrom
    {
        return source.OverrideInput((TTo to) => (TFrom)to);
    }

    public static string GetMemberName<TSource, TResult>(this Expression<Func<TSource, TResult>> expr)
    {
        if (expr == null) throw new ArgumentNullException(nameof(expr));

        return expr.Body.GetMember().Select(member => member.Name)
                   .GetValue(() => new ArgumentException("not member expression", nameof(expr)));
    }

    public static Expression<Action<TSource, TProperty>> ToSetLambdaExpression<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expr)
    {
        return expr.GetProperty().ToSetLambdaExpression<TSource, TProperty>();
    }

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


    private class NodeExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _startNode;

        private readonly List<NodeExpressionVisitor> ChildVisitors = new List<NodeExpressionVisitor>();

        public NodeExpressionVisitor(Expression startNode)
        {
            if (startNode == null) throw new ArgumentNullException(nameof(startNode));

            this._startNode = startNode;
        }

        public override Expression? Visit(Expression? node)
        {
            if (node == null || node == this._startNode)
            {
                return base.Visit(node);
            }
            else
            {
                var childVisitor = new NodeExpressionVisitor(node);
                this.ChildVisitors.Add(childVisitor);
                return childVisitor.Visit(node);
            }
        }

        public Node<Expression> ToNode()
        {
            return new Node<Expression>(this._startNode, this.ChildVisitors.Select(child => child.ToNode()));
        }
    }
}
