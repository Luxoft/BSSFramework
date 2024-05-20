using System.Linq.Expressions;

using Framework.Core;
using Framework.Persistent;

namespace Framework.DomainDriven._Visitors;

public class ExpressionVisitorContainerDefaultItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return OptimizeBooleanLogicVisitor.Value;
        yield return OptimizeWhereAndConcatVisitor.Value;

        yield return RestoreQueryableCallsVisitor.Value;

        yield return OverrideHasFlagVisitor.Value;
        yield return ExpandPathVisitor.Value;
        yield return EscapeUnderscoreVisitor.Value;
    }
}
