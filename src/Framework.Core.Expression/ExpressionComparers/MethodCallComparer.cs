using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers
{
    internal class MethodCallComparer : ExpressionComparer<MethodCallExpression>
    {
        private MethodCallComparer()
        {

        }



        public override bool Equals(MethodCallExpression x, MethodCallExpression y)
        {
            return base.Equals(x, y)
                && x.Method == y.Method
                && ExpressionComparer.Value.Equals(x.Object, y.Object)
                && x.Arguments.SequenceEqual(y.Arguments,
                ExpressionComparer.Value);
        }

        public override int GetHashCode(MethodCallExpression obj)
        {
            return base.GetHashCode(obj) ^ obj.Arguments.Count;
        }

        public static readonly MethodCallComparer Value = new MethodCallComparer();
    }
}