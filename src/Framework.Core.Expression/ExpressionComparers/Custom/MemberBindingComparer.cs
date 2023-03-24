using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers;

internal class MemberBindingComparer : IEqualityComparer<MemberBinding>
{
    private MemberBindingComparer()
    {

    }


    public bool Equals(MemberBinding preX, MemberBinding preY)
    {
        var baseEquals = preX.BindingType == preY.BindingType && preX.Member == preY.Member;

        return baseEquals &&

               ((from x in (preX as MemberAssignment).ToMaybe()

                 from y in (preY as MemberAssignment).ToMaybe()

                 select ExpressionComparer.Value.Equals(x.Expression, y.Expression)))

               .Or(() => from x in (preX as MemberListBinding).ToMaybe()

                         from y in (preY as MemberListBinding).ToMaybe()

                         select x.Initializers.SequenceEqual(y.Initializers, ElementInitComparer.Value))

               .Or(() => from x in (preX as MemberMemberBinding).ToMaybe()

                         from y in (preY as MemberMemberBinding).ToMaybe()

                         select x.Bindings.SequenceEqual(y.Bindings, this))

               .GetValueOrDefault();
    }

    public int GetHashCode(MemberBinding memberBinding)
    {
        return memberBinding.BindingType.GetHashCode() ^ memberBinding.Member.GetHashCode();
    }


    public static readonly MemberBindingComparer Value = new MemberBindingComparer();
}
