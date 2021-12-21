using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers
{
    internal class ParameterComparer : ExpressionComparer<ParameterExpression>
    {
        private ParameterComparer()
        {

        }

        public override bool Equals(ParameterExpression x, ParameterExpression y)
        {
            return base.Equals(x, y)
                && x.Name == y.Name;
        }


        public static readonly ParameterComparer Value = new ParameterComparer();
    }
}