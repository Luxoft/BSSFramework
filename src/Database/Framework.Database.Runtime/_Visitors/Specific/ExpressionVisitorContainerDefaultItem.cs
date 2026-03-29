using System.Linq.Expressions;

using CommonFramework.Visitor;

using Framework.Database.ExpressionVisitorContainer;

namespace Framework.Database._Visitors.Specific;

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
