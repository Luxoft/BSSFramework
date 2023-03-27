using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers;

internal class LambdaComparer : ExpressionComparer<LambdaExpression>
{
    private LambdaComparer()
    {

    }



    public override bool Equals(LambdaExpression x, LambdaExpression y)
    {
        return base.Equals(x, y)
               && //object.ReferenceEquals(x, y) ||
               (x.Parameters.SequenceEqual(y.Parameters, ParameterComparer.Value)
                && ExpressionComparer.Value.Equals(x.Body, y.Body));
    }

    public override int GetHashCode(LambdaExpression obj)
    {
        return base.GetHashCode(obj) ^ obj.Parameters.Count;
    }

    public static readonly LambdaComparer Value = new LambdaComparer();
}
