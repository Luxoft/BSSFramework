using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers;

internal class MemberComparer : ExpressionComparer<MemberExpression>
{
    private MemberComparer()
    {

    }



    public override bool Equals(MemberExpression x, MemberExpression y)
    {
        return base.Equals(x, y)
               && x.Member == y.Member
               && ExpressionComparer.Value.Equals(x.Expression, y.Expression);
    }

    public override int GetHashCode(MemberExpression obj)
    {
        return base.GetHashCode(obj) ^ obj.Member.GetHashCode();
    }

    public static readonly MemberComparer Value = new MemberComparer();
}
