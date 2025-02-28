using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Framework.GenericQueryable.Default;

public class DefaultGenericQueryableVisitor : ExpressionVisitor
{
    [return: NotNullIfNotNull("node")]
    public override Expression? Visit(Expression? node)
    {
        if (node is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            return new GenericQueryableExecuteExpression((LambdaExpression)this.Visit(genericQueryableExecuteExpression.CallExpression));
        }
        else
        {
            return base.Visit(node);
        }
    }
}
