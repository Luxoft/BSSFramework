using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers
{
    internal class UnaryComparer : ExpressionComparer<UnaryExpression>
    {
        private UnaryComparer()
        {

        }



        public override bool Equals(UnaryExpression x, UnaryExpression y)
        {
            return base.Equals(x, y)
                && x.Method == y.Method
                && ExpressionComparer.Value.Equals(x.Operand, y.Operand);
        }


        public static readonly UnaryComparer Value = new UnaryComparer();
    }
}