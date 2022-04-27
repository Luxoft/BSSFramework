using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Framework.Core;

public class VisitedQueryable : IOrderedQueryable
{
    private readonly IQueryable baseQueryable;

    private readonly ExpressionVisitor visitor;

    public VisitedQueryable([NotNull] IQueryable baseQueryable, [NotNull] ExpressionVisitor visitor)
    {
        this.baseQueryable = baseQueryable ?? throw new ArgumentNullException(nameof(baseQueryable));
        this.visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        var newExpr = this.visitor.Visit(this.Expression);

        var newQ = this.baseQueryable.Provider.CreateQuery(newExpr);

        return newQ.GetEnumerator();
    }

    public Expression Expression => this.baseQueryable.Expression;

    public Type ElementType => this.baseQueryable.ElementType;

    public IQueryProvider Provider => new VisitedQueryProvider(this.baseQueryable.Provider, this.visitor);
}

public class VisitedQueryable<TElement> : VisitedQueryable, IOrderedQueryable<TElement>
{
    private readonly IQueryable<TElement> baseQueryable;

    private readonly ExpressionVisitor visitor;

    public VisitedQueryable([NotNull] IQueryable<TElement> baseQueryable, [NotNull] ExpressionVisitor visitor)
        : base(baseQueryable, visitor)
    {
        this.baseQueryable = baseQueryable ?? throw new ArgumentNullException(nameof(baseQueryable));
        this.visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
    }


    public IEnumerator<TElement> GetEnumerator()
    {
        var newExpr = this.visitor.Visit(this.Expression);

        var newQ = this.baseQueryable.Provider.CreateQuery<TElement>(newExpr);

        return newQ.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.baseQueryable.GetEnumerator();
    }
}
