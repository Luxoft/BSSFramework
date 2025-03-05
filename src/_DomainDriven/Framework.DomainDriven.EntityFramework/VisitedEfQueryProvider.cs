using System.Linq.Expressions;

using Framework.GenericQueryable;

using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Framework.DomainDriven.EntityFramework;

public class VisitedEfQueryProvider(IQueryCompiler queryCompiler) : EntityQueryProvider(queryCompiler)
{
    public ExpressionVisitor Visitor { get; set; }

    public IGenericQueryableExecutor GenericQueryableExecutor { get; set; }

    public override TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
    {
        if (expression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            return (TResult)this.GenericQueryableExecutor.Execute(genericQueryableExecuteExpression);
        }
        else
        {
            return base.ExecuteAsync<TResult>(expression, cancellationToken);
        }
    }

    public override TResult Execute<TResult>(Expression expression)
    {
        if (expression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            return (TResult)this.GenericQueryableExecutor.Execute(genericQueryableExecuteExpression);
        }
        else
        {
            return base.Execute<TResult>(expression);
        }
    }

    public override object Execute(Expression expression)
    {
        if (expression is GenericQueryableExecuteExpression genericQueryableExecuteExpression)
        {
            return this.GenericQueryableExecutor.Execute(genericQueryableExecuteExpression);
        }
        else
        {
            return base.Execute(expression);
        }
    }
}
