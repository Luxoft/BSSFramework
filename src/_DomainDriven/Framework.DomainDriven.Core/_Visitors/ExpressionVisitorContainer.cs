using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven;

public class ExpressionVisitorContainer : IExpressionVisitorContainer
{
    public ExpressionVisitorContainer(IEnumerable<ExpressionVisitor> expressionVisitors)
    {
        this.Visitor = expressionVisitors.ToComposite();
    }

    public ExpressionVisitor Visitor { get; }
}
