using System.Linq.Expressions;
using Framework.Database;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SampleSystem.WebApiCore.DependencyInjection;

public class GeneralQueryExpressionInterceptor(IExpressionVisitorContainer expressionVisitorContainer) : IQueryExpressionInterceptor
{
    public Expression QueryCompilationStarting(Expression queryExpression, QueryExpressionEventData eventData) => expressionVisitorContainer.Visitor.Visit(queryExpression);
}
