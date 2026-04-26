using System.Linq.Expressions;

using Anch.Core.Visitor;

namespace Framework.Database.Visitors.Containers;

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
