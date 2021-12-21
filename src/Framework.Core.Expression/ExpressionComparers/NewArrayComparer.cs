using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers
{
    internal class NewArrayComparer : ExpressionComparer<NewArrayExpression>
    {
        private NewArrayComparer()
        {

        }



        public override bool Equals(NewArrayExpression x, NewArrayExpression y)
        {
            return base.Equals(x, y)
                   && x.Expressions.SequenceEqual(y.Expressions, ExpressionComparer.Value);
        }


        public static readonly NewArrayComparer Value = new NewArrayComparer();
    }
}