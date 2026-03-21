using System.Linq.Expressions;

using CommonFramework.Visitor;

using Framework.Application._Visitors.ExpressionVisitorContainer;

namespace Framework.Application._Visitors.Specific;

public class ExpressionVisitorContainerDefaultItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return OptimizeBooleanLogicVisitor.Value;
        yield return SquashWhereQueryableVisitor.Value;

        yield return RestoreQueryableCallsVisitor.Value;

        yield return OverrideHasFlagVisitor.Value;
        yield return EscapeUnderscoreVisitor.Value;
    }
}
