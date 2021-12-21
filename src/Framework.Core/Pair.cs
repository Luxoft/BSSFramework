using System;

namespace Framework.Core
{
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "", Name = "PairOf{0}And{1}")]
    [Obsolete("v10 Use Tuple")]
    public class Pair<T1, T2>
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public T1 Value1 { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public T2 Value2 { get; set; }

        public Pair(T1 value1, T2 value2)
        {
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public Pair()
        {
        }
    }
    public static class Pair
    {
        [Obsolete("v10 Use Tuple")]
        public static Pair<T1, T2> Create<T1, T2>(T1 value1, T2 value2)
        {
            return new Pair<T1, T2>(value1, value2);
        }
    }
}
