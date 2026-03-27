using System.Linq.Expressions;

namespace Framework.Database.NHibernate.Envers.LinqVisitors.Visitors;

internal class EqualEvaluator : ExpressionVisitor
{
    private readonly Immutable<string> value = new();
    private readonly Immutable<global::NHibernate.Envers.Query.Criteria.AuditProperty> property = new();

    public string Value
    {
        get { return this.value.Value; }
    }

    public global::NHibernate.Envers.Query.Criteria.AuditProperty Property
    {
        get { return this.property.Value; }
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (!this.value.IsInit)
        {
            this.value.Value = node.Value.ToString();
        }
        return base.VisitConstant(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression.GetType() == typeof(ConstantExpression))
        {
            this.value.Value = node.ToValue();
        }
        else
        {
            this.property.Value = node.ToAuditProperty();
        }
        return base.VisitMember(node);
    }
}
