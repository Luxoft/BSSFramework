using System.Linq.Expressions;
using NHibernate.Envers.Query.Criteria;

namespace NHibernate.Linq.Visitors
{
    internal class EqualEvaluator : ExpressionVisitor
    {
        private readonly Immutable<string> value;
        private readonly Immutable<AuditProperty> property;

        public EqualEvaluator()
        {
            this.value = new Immutable<string>();
            this.property = new Immutable<AuditProperty>();
        }

        public string Value
        {
            get { return this.value.Value; }
        }

        public AuditProperty Property
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
}
