using System.Linq.Expressions;

using Framework.GenericQueryable;

using NHibernate;
using NHibernate.Engine;
using NHibernate.Linq;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// NHibnate-провайдер доступа, который применяет Visitor-ы для Expression-ов
/// </summary>
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

    public IGenericQueryableExecutor GenericQueryableExecutor { get; set; }

    protected override IQueryProvider CreateWithOptions(NhQueryableOptions options)
    {
        return new VisitedQueryProvider(this.Session, this.Collection, options)
               {
                   Visitor = this.Visitor, GenericQueryableExecutor = this.GenericQueryableExecutor
               };
    }

    private Expression TryApplyVisitor(Expression expression)
    {
        return this.Visitor == null ? expression : this.Visitor.Visit(expression);
    }

    protected override NhLinqExpression PrepareQuery(Expression expression, out IQuery query)
    {
        return base.PrepareQuery(this.TryApplyVisitor(expression), out query);
    }

    public override IQueryable CreateQuery(Expression expression)
    {
        return base.CreateQuery(this.TryApplyVisitor(expression));
    }

    public override IQueryable<T> CreateQuery<T>(Expression expression)
    {
        return base.CreateQuery<T>(this.TryApplyVisitor(expression));
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
