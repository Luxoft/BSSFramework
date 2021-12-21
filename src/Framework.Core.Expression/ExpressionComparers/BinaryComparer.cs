using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers
{
    internal class BinaryComparer : ExpressionComparer<BinaryExpression>
    {
        private BinaryComparer()
        {

        }



        public override bool Equals(BinaryExpression x, BinaryExpression y)
        {
            return base.Equals(x, y)
                && x.Method == y.Method
                && ExpressionComparer.Value.Equals(x.Left, y.Left)
                && ExpressionComparer.Value.Equals(x.Right, y.Right);
        }


        public static readonly BinaryComparer Value = new BinaryComparer();
    }
}