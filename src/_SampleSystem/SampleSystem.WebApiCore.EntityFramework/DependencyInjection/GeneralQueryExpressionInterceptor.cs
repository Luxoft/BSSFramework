using System.Linq.Expressions;

using Framework.Database.ExpressionVisitorContainer;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SampleSystem.WebApiCore.DependencyInjection;

public class GeneralQueryExpressionInterceptor(IExpressionVisitorContainer expressionVisitorContainer) : IQueryExpressionInterceptor
{
    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        return expressionVisitorContainer.Visitor.Visit(queryExpression);
    }
}
