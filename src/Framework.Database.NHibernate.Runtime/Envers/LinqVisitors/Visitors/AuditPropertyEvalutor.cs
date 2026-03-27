using System.Linq.Expressions;

using NHibernate.Envers.Query;

namespace Framework.Database.NHibernate.Envers.LinqVisitors.Visitors;

internal class AuditPropertyEvalutor : ExpressionVisitor
{
    private readonly Immutable<global::NHibernate.Envers.Query.Criteria.AuditProperty> result = new();

    public global::NHibernate.Envers.Query.Criteria.AuditProperty AuditProperty
    {
        get { return this.result.Value; }
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        this.result.Value =  AuditEntity.Property(node.Member.Name);
        return base.VisitMember(node);
    }
}
