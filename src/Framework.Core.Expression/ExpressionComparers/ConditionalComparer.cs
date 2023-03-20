using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers;

internal class ConditionalComparer : ExpressionComparer<ConditionalExpression>
{
    public override bool Equals(ConditionalExpression x, ConditionalExpression y)
    {
        return base.Equals(x, y)
               && ExpressionComparer.Value.Equals(x.Test, y.Test)
               && ExpressionComparer.Value.Equals(x.IfTrue, y.IfTrue)
               && ExpressionComparer.Value.Equals(x.IfFalse, y.IfFalse);
    }


    public static readonly ConditionalComparer Value = new ConditionalComparer();
}
