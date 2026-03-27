using System.Linq.Expressions;

using Framework.BLL.Domain.Persistent.Visitors;
using Framework.Database.ExpressionVisitorContainer;

namespace Framework.BLL;

public class BLLExpressionVisitorContainerItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return ExpandPathVisitor.Value;
    }
}
