using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core;

public static class InlineEvalExpressionExtensions
{
    /// <summary>
    /// Встраивает вызовы Eval-методов непосредственно в сам текущий Expression
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expr"></param>
    /// <returns></returns>
    public static Expression<T> InlineEval<T>(this Expression<T> expr)
    {
        return expr.UpdateBody(new InlineEvalExpressionVisitor());
    }

    private sealed class InlineEvalExpressionVisitor : ExpressionVisitor
    {
        private static readonly MethodInfo[] EvalMethods =
        {
                new Func<Expression<Func<object, object, object, object>>, object, object, object, object>(ExpressionExtensions.Eval).Method.GetGenericMethodDefinition(),
                new Func<Expression<Func<object, object, object>>, object, object, object>(ExpressionExtensions.Eval).Method.GetGenericMethodDefinition(),
                new Func<Expression<Func<object, object>>, object, object>(ExpressionExtensions.Eval).Method.GetGenericMethodDefinition(),
                new Func<Expression<Func<object>>, object>(ExpressionExtensions.Eval).Method.GetGenericMethodDefinition(),
        };

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var baseVisited = base.VisitMethodCall(node);

            return TryInlineEval(baseVisited)
                       .Or(() => this.VisitExpressionArguments(baseVisited))
                       .GetValueOrDefault(baseVisited);
        }

        private static Maybe<Expression> TryInlineEval(Expression baseNode)
        {
            return from node in (baseNode as MethodCallExpression).ToMaybe()

                   where EvalMethods.Any(evalMethod => node.Method.IsGenericMethodImplementation(evalMethod))

                   from evalLambda in node.Arguments.First().GetMemberConstValue<LambdaExpression>()

                   select node.Arguments.Skip(1)
                              .ZipStrong(evalLambda.Parameters, (arg, param) => new { arg, param })
                              .Aggregate(evalLambda.Body, (state, pair) => state.Override(pair.param, pair.arg));
        }

        private Maybe<Expression> VisitExpressionArguments(Expression baseNode)
        {
            return from node in (baseNode as MethodCallExpression).ToMaybe()

                   let visitedArguments = node.Arguments.ToList(arg => new { arg, visitedArg = this.Visit(arg) })

                   where visitedArguments.Any(pair => pair.arg != pair.visitedArg)

                   select (Expression)Expression.Call(node.Object, node.Method, visitedArguments.ToArray(pair => pair.visitedArg));
        }
    }
}
