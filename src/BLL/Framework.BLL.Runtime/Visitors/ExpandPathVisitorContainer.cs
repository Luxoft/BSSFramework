using System.Linq.Expressions;

using Framework.BLL.Services;
using Framework.Database;

namespace Framework.BLL.Visitors;

public class ExpandPathVisitorContainer(IPropertyPathService propertyPathService) : IExpressionVisitorContainer
{
    public ExpressionVisitor Visitor { get; } = new ExpandPathVisitor(propertyPathService);
}
