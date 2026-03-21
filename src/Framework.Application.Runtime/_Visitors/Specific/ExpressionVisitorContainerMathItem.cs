using System.Linq.Expressions;

using Framework.Application._Visitors.ExpressionVisitorContainer;

namespace Framework.Application._Visitors.Specific;

public class ExpressionVisitorContainerMathItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return new OverrideMethodInfoVisitor<Func<int, int, int>>(
            Math.Max,
            (v1, v2) => v1 > v2 ? v1 : v2);

        yield return new OverrideMethodInfoVisitor<Func<int, int, int>>(
            Math.Min,
            (v1, v2) => v1 < v2 ? v1 : v2);
    }
}
