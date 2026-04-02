using System.Linq.Expressions;

namespace Framework.Database._Visitors.Containers;

public class MathExpressionVisitorContainer : ExpressionVisitorAggregator
{
    protected override IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return new OverrideMethodInfoVisitor<Func<int, int, int>>(
            Math.Max,
            (v1, v2) => v1 > v2 ? v1 : v2);

        yield return new OverrideMethodInfoVisitor<Func<int, int, int>>(
            Math.Min,
            (v1, v2) => v1 < v2 ? v1 : v2);
    }
}
