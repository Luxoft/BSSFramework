using System.Linq.Expressions;

using Framework.BLL.Domain.Persistent.Visitors;
using Framework.Database;

namespace Framework.BLL;

public class ExpandPathVisitorContainer : IExpressionVisitorContainer
{
    public ExpressionVisitor Visitor { get; } = ExpandPathVisitor.Value;
}
