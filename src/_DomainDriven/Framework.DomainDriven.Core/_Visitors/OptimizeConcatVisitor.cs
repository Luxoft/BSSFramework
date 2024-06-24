using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven._Visitors;

internal class OptimizeWhereAndConcatVisitor : ExpressionVisitor
{
    private static readonly MethodInfo GenericConcatMethod = new Func<IQueryable<object>, IEnumerable<object>, IQueryable<object>>(Queryable.Concat).Method.GetGenericMethodDefinition();

    private static readonly MethodInfo GenericWhereMethod = new Func<IQueryable<object>, Expression<Func<object, bool>>, IQueryable<object>>(Queryable.Where).Method.GetGenericMethodDefinition();

    private static readonly MethodInfo GenericBuildOrMethod = new Func<Expression<Func<object, bool>>, Expression<Func<object, bool>>, Expression<Func<object, bool>>>(ExpressionExtensions.BuildOr).Method.GetGenericMethodDefinition();

    private static readonly MethodInfo GenericBuildAndMethod = new Func<Expression<Func<object, bool>>, Expression<Func<object, bool>>, Expression<Func<object, bool>>>(ExpressionExtensions.BuildAnd).Method.GetGenericMethodDefinition();



    private OptimizeWhereAndConcatVisitor()
    {

    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        return this.TryOptimizeWhere(node).Or(() => this.TryOptimizeConcat(node))
                   .GetValueOrDefault(() => base.VisitMethodCall(node));
    }

    private Maybe<Expression> TryOptimizeConcat(MethodCallExpression node)
    {
        return from method in node.Method.ToMaybe()

               where method.IsGenericMethodImplementation(GenericConcatMethod)

               let elementType = method.GetGenericArguments().Single()

               from q1 in (this.Visit(node.Arguments[0]) as MethodCallExpression).ToMaybe()

               from q2 in (this.Visit(node.Arguments[1]) as MethodCallExpression).ToMaybe()

               where q1.Arguments[0] == q2.Arguments[0]

               let whereMethod = GenericWhereMethod.MakeGenericMethod(elementType)

               where q1.Method == whereMethod && q2.Method == whereMethod

               from q1FilterQuote in (q1.Arguments[1] as UnaryExpression).ToMaybe()

               from q2FilterQuote in (q2.Arguments[1] as UnaryExpression).ToMaybe()

               where q1FilterQuote.NodeType == ExpressionType.Quote && q2FilterQuote.NodeType == ExpressionType.Quote

               let buildOrMethod = GenericBuildOrMethod.MakeGenericMethod(elementType)

               let newFilter = (Expression)buildOrMethod.Invoke(null, new object[] { q1FilterQuote.Operand, q2FilterQuote.Operand })

               select (Expression)Expression.Call(whereMethod, q1.Arguments[0], Expression.Quote(newFilter));
    }

    private Maybe<Expression> TryOptimizeWhere(MethodCallExpression node)
    {
        return from method in node.Method.ToMaybe()

               where method.IsGenericMethodImplementation(GenericWhereMethod)

               let elementType = method.GetGenericArguments().Single()

               from innerCallExpr in (this.Visit(node.Arguments[0]) as MethodCallExpression).ToMaybe()

               where method == innerCallExpr.Method

               from q1FilterQuote in (innerCallExpr.Arguments[1] as UnaryExpression).ToMaybe()

               from q2FilterQuote in (node.Arguments[1] as UnaryExpression).ToMaybe()

               let buildAndMethod = GenericBuildAndMethod.MakeGenericMethod(elementType)

               let newFilter = (Expression)buildAndMethod.Invoke(null, new object[] { q1FilterQuote.Operand, q2FilterQuote.Operand })

               select (Expression)Expression.Call(method, innerCallExpr.Arguments[0], Expression.Quote(newFilter));
    }

    public static readonly OptimizeWhereAndConcatVisitor Value = new OptimizeWhereAndConcatVisitor();
}
