using System.Linq.Expressions;

using Framework.Database.NHibernate.Envers.LinqVisitors.Visitors;

using NHibernate.Envers.Query.Criteria;

namespace Framework.Database.NHibernate.Envers.LinqVisitors;

public static class EvaluateExtension
{
    public static IAuditCriterion ToCriterion(this Expression expression)
    {
        var visitor = new Visitor();
        visitor.Visit(expression);
        return visitor.Criterion;
    }

    internal static global::NHibernate.Envers.Query.Criteria.AuditProperty ToAuditProperty(this Expression expression)
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
