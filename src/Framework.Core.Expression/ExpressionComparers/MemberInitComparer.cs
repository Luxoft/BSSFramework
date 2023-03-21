using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers;

internal class MemberInitComparer : ExpressionComparer<MemberInitExpression>
{
    public override bool Equals(MemberInitExpression x, MemberInitExpression y)
    {
        return base.Equals(x, y)
               && ExpressionComparer.Value.Equals(x.NewExpression, y.NewExpression)
               && x.Bindings.SequenceEqual(y.Bindings, MemberBindingComparer.Value);
    }


    public static readonly MemberInitComparer Value = new MemberInitComparer();
}
