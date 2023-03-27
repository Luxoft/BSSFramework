using System.Linq.Expressions;

namespace Framework.Core;

public class OverrideExpressionVisitor : ExpressionVisitor
{
    private readonly Func<Expression, bool> _isReplaceExpression;
    private readonly Expression _newExpression;


    public OverrideExpressionVisitor(Func<Expression, bool> isReplaceExpression, Expression newExpression)
    {
        if (isReplaceExpression == null) throw new ArgumentNullException(nameof(isReplaceExpression));
        if (newExpression == null) throw new ArgumentNullException(nameof(newExpression));

        this._isReplaceExpression = isReplaceExpression;
        this._newExpression = newExpression;
    }

    public OverrideExpressionVisitor(Expression oldExpression, Expression newExpression)
    {
        if (oldExpression == null) throw new ArgumentNullException(nameof(oldExpression));
        if (newExpression == null) throw new ArgumentNullException(nameof(newExpression));

        this._isReplaceExpression = node => node == oldExpression;
        this._newExpression = newExpression;
    }

    public override Expression Visit(Expression node)
    {
        return this._isReplaceExpression(node) ? this._newExpression : base.Visit(node);
    }
}
