using System;
using System.Linq;
using System.Linq.Expressions;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Linq;

public class VisitedQueryProvider : DefaultQueryProvider
{
    public VisitedQueryProvider(ISessionImplementor session)
            : base(session)
    {
    }

    protected VisitedQueryProvider(ISessionImplementor session, object collection, NhQueryableOptions options)
            : base(session, collection, options)
    {
    }

    public ExpressionVisitor Visitor { get; set; }

    protected override IQueryProvider CreateWithOptions(NhQueryableOptions options)
    {
        return new VisitedQueryProvider(this.Session, this.Collection, options) { Visitor = this.Visitor };
    }

    protected override NhLinqExpression PrepareQuery(Expression expression, out IQuery query)
    {
        return base.PrepareQuery(this.Visitor == null ? expression : this.Visitor.Visit(expression), out query);
    }
}
