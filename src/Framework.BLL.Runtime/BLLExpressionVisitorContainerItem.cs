using System.Linq.Expressions;

using Framework.Application._Visitors.ExpressionVisitorContainer;
using Framework.BLL.Domain.Persistent.Visitors;

namespace Framework.BLL;

public class BLLExpressionVisitorContainerItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return ExpandPathVisitor.Value;
    }
}
