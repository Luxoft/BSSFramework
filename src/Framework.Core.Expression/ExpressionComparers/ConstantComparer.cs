using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers;

internal class ConstantComparer : ExpressionComparer<ConstantExpression>
{
    private ConstantComparer()
    {

    }



    public override bool Equals(ConstantExpression x, ConstantExpression y)
    {
        return base.Equals(x, y) && object.Equals(x.Value, y.Value);
    }

    public override int GetHashCode(ConstantExpression obj)
    {
        return base.GetHashCode(obj);
    }

    public static readonly ConstantComparer Value = new ConstantComparer();
}
