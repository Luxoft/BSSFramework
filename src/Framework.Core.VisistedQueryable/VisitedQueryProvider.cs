using System;
using System.Linq;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.Core;

public class VisitedQueryProvider : IQueryProvider
{
    private readonly IQueryProvider baseQueryProvider;

    private readonly ExpressionVisitor visitor;

    public VisitedQueryProvider([NotNull] IQueryProvider baseQueryProvider, [NotNull] ExpressionVisitor visitor)
    {
        this.visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
        this.baseQueryProvider = baseQueryProvider ?? throw new ArgumentNullException(nameof(baseQueryProvider));
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new VisitedQueryable(this.baseQueryProvider.CreateQuery(this.visitor.Visit(expression)), this.visitor);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new VisitedQueryable<TElement>(this.baseQueryProvider.CreateQuery<TElement>(this.visitor.Visit(expression)), this.visitor);
    }

    public object Execute(Expression expression)
    {
        return this.baseQueryProvider.Execute(this.visitor.Visit(expression));
    }

    public TResult Execute<TResult>(Expression expression)
    {
        var overrideExpression = this.visitor.Visit(expression);

        return this.baseQueryProvider.Execute<TResult>(overrideExpression);
    }
}
