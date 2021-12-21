using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;

namespace Framework.Core.ExpressionComparers
{
    internal class ElementInitComparer : IEqualityComparer<ElementInit>
    {
        private ElementInitComparer()
        {

        }


        public bool Equals(ElementInit x, ElementInit y)
        {
            return x.AddMethod == y.AddMethod
                && x.Arguments.SequenceEqual(y.Arguments, ExpressionComparer.Value);
        }

        public int GetHashCode(ElementInit obj)
        {
            return obj.AddMethod.GetHashCode();
        }


        public static readonly ElementInitComparer Value = new ElementInitComparer();
    }
}