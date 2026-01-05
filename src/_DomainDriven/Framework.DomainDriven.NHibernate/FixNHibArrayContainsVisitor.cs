using System.Linq.Expressions;
using System.Reflection;

using CommonFramework;

namespace Framework.DomainDriven.NHibernate;

public class FixNHibArrayContainsVisitor : ExpressionVisitor
{
    private static readonly IReadOnlySet<MethodInfo> ArrayContainsMethods =
        new HashSet<MethodInfo>
        {
            new Func<ReadOnlySpan<Ignore>, Ignore, bool>(MemoryExtensions.Contains).Method.GetGenericMethodDefinition(),
            new Func<ReadOnlySpan<Ignore>, Ignore, IEqualityComparer<Ignore>, bool>(MemoryExtensions.Contains).Method.GetGenericMethodDefinition()
        };

    private static readonly MethodInfo EnumerableContainsMethod = new Func<IEnumerable<Ignore>, Ignore, bool>(Enumerable.Contains).Method.GetGenericMethodDefinition();

    public override Expression? Visit(Expression? node)
    {
        if (node is MethodCallExpression { Method: { IsGenericMethod: true } method } callExpr
            && ArrayContainsMethods.Contains(method.GetGenericMethodDefinition())
            && callExpr.Arguments[0] is MethodCallExpression { Method: var castMethod } castExpr
            && castMethod.Name == "op_Implicit")
        {
            var newNode = Expression.Call(EnumerableContainsMethod.MakeGenericMethod(method.GetGenericArguments()), castExpr.Arguments.Single(), callExpr.Arguments[1]);

            return base.Visit(newNode);
        }

        return base.Visit(node);
    }

    public static readonly FixNHibArrayContainsVisitor Value = new();
}
