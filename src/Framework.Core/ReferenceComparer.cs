using System.Collections.Generic;

namespace Framework.Core
{
    public class ReferenceComparer<T> : IEqualityComparer<T>
        where T : class
    {
        private ReferenceComparer()
        {

        }


        public bool Equals(T x, T y)
        {
            return object.ReferenceEquals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.Maybe(v => v.GetHashCode());
        }


        public static readonly ReferenceComparer<T> Value = new ReferenceComparer<T>();
    }
}