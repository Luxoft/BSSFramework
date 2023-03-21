using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers;

internal class NewComparer : ExpressionComparer<NewExpression>
{
    public override bool Equals(NewExpression x, NewExpression y)
    {
        return base.Equals(x, y)

               && x.Arguments.SequenceEqual(y.Arguments, ExpressionComparer.Value)

               && ((x.Members == null && y.Members == null)

                   || (x.Members != null && y.Members != null && x.Members.SequenceEqual(y.Members)));
    }


    public static readonly NewComparer Value = new NewComparer();
}
