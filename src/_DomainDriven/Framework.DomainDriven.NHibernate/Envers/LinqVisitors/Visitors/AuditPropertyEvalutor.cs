using System.Linq.Expressions;

using NHibernate.Envers.Query;
using NHibernate.Envers.Query.Criteria;

namespace NHibernate.Linq.Visitors;

internal class AuditPropertyEvalutor : ExpressionVisitor
{
    private readonly Immutable<AuditProperty> result;

    public AuditPropertyEvalutor()
    {
        this.result = new Immutable<AuditProperty>();
    }

    public AuditProperty AuditProperty
    {
        get { return this.result.Value; }
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        this.result.Value =  AuditEntity.Property(node.Member.Name);
        return base.VisitMember(node);
    }
}
