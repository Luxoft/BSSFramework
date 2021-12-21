using System.Linq.Expressions;

using NHibernate.Envers.Query.Criteria;

namespace NHibernate.Linq.Visitors
{
    public static class EvaluateExtension
    {
        public static IAuditCriterion ToCriterion(this Expression expression)
        {
            var visitor = new Visitor();
            visitor.Visit(expression);
            return visitor.Criterion;
        }

        internal static AuditProperty ToAuditProperty(this Expression expression)
        {
            var evaluator = new AuditPropertyEvalutor();
            evaluator.Visit(expression);
            return evaluator.AuditProperty;
        }

        internal static string ToValue(this Expression expression)
        {
            var visitor = new ValueEvaluatorVisitor();
            visitor.Visit(expression);
            return visitor.EvaluatedValue;
        }
    }
}
