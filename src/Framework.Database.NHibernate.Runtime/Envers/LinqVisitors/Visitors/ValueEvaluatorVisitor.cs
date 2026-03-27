using System.Linq.Expressions;

namespace Framework.Database.NHibernate.Envers.LinqVisitors.Visitors;

internal class ValueEvaluatorVisitor : ExpressionVisitor
{
    private readonly Immutable<string> result = new();

    public string EvaluatedValue => this.result.Value;

    protected override Expression VisitConstant(ConstantExpression node)
    {
        this.result.Value = node.Value.ToString();
        return base.VisitConstant(node);
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        var constantExpression = (ConstantExpression)node.Expression;

        var anonymousObject = constantExpression.Value;

        var anonymousType = anonymousObject.GetType();

        var fieldInfo = anonymousType.GetField(node.Member.Name);

        this.result.Value = fieldInfo.GetValue(anonymousObject).ToString();

        return node;
    }
}
