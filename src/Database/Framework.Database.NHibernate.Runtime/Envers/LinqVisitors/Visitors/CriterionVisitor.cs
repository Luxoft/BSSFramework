using System.Linq.Expressions;

using NHibernate.Criterion;
using NHibernate.Envers.Query;
using NHibernate.Envers.Query.Criteria;

using Expression = System.Linq.Expressions.Expression;

namespace Framework.Database.NHibernate.Envers.LinqVisitors.Visitors;

internal class CriterionVisitor : ExpressionVisitor
{
    private readonly Immutable<IAuditCriterion> criterion = new();

    public IAuditCriterion Criterion => this.criterion.Value;

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        switch (node.Method.Name)
        {
            case "StartsWith":
                this.criterion.Value = this.GetLikeCriterion(node, MatchMode.Start);
                break;
            case "EndsWith":
                this.criterion.Value = this.GetLikeCriterion(node, MatchMode.End);
                break;
            case "Contains":
                this.criterion.Value = this.GetLikeCriterion(node, MatchMode.Anywhere);
                break;
            default:
                throw new NotImplementedException(node.Method.Name);
        }

        return node;
    }

    private IAuditCriterion GetLikeCriterion(MethodCallExpression node, MatchMode matchMode)
    {
        var auditProperty = node.Object.ToAuditProperty();
        var value = node.Arguments[0].ToValue();
        return auditProperty.Like(value, matchMode);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        switch (node.NodeType)
        {
            case ExpressionType.AndAlso:
                this.VisitBinaryExpression(node, AuditEntity.And);
                break;

            case ExpressionType.OrElse:
                this.VisitBinaryExpression(node, AuditEntity.Or);
                break;

            case ExpressionType.Equal:
                var equalEvaluator = new EqualEvaluator();
                equalEvaluator.Visit(node.Left);
                equalEvaluator.Visit(node.Right);
                this.criterion.Value = equalEvaluator.Property.Eq(equalEvaluator.Value);
                break;

            default:
                throw new NotImplementedException(node.NodeType.ToString());
        }

        return node;
    }

    private void VisitBinaryExpression(BinaryExpression expr, Func<IAuditCriterion, IAuditCriterion, IAuditCriterion> evaluateFunc) => this.criterion.Value = evaluateFunc(expr.Left.ToCriterion(), expr.Right.ToCriterion());

    protected override Expression VisitUnary(UnaryExpression node) => node;
}
