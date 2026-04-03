using System.Linq.Expressions;

using CommonFramework.Visitor;

namespace Framework.Database._Visitors.Containers;

public class DefaultExpressionVisitorContainer : ExpressionVisitorAggregator
{
    protected override IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return OptimizeBooleanLogicVisitor.Value;
        yield return SquashWhereQueryableVisitor.Value;

        yield return RestoreQueryableCallsVisitor.Value;

        yield return OverrideHasFlagVisitor.Value;
        yield return EscapeUnderscoreVisitor.Value;
    }
}
