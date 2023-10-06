using System.Linq.Expressions;

using NHibernate.Linq;

namespace Framework.AutomationCore.Unit.Queryable;

internal class ExpressionTreeModifier : ExpressionVisitor
{
    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (!IsFetchMethod(node.Method.Name)) return base.VisitMethodCall(node);

        var fetchInput = this.AvoidFetch(node);
        return fetchInput.NodeType switch
        {
            ExpressionType.Constant => this.VisitConstant((ConstantExpression)fetchInput),
            _ => fetchInput
        };
    }

    private Expression AvoidFetch(MethodCallExpression node) =>
        IsFetchMethod(node.Method.Name) ? this.AvoidFetch((node.Arguments[0] as MethodCallExpression)!) : node;

    private static bool IsFetchMethod(string name) =>
        nameof(EagerFetchingExtensionMethods.Fetch).Equals(name)
        || nameof(EagerFetchingExtensionMethods.FetchMany).Equals(name)
        || nameof(EagerFetchingExtensionMethods.ThenFetch).Equals(name)
        || nameof(EagerFetchingExtensionMethods.ThenFetchMany).Equals(name);
}
