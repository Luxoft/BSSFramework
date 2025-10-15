using System.Linq.Expressions;

using Framework.DomainDriven._Visitors;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SampleSystem.ServiceEnvironment;

public class GeneralQueryExpressionInterceptor(IExpressionVisitorContainer expressionVisitorContainer) : IQueryExpressionInterceptor
{
    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData)
    {
        return expressionVisitorContainer.Visitor.Visit(queryExpression);
    }
}
