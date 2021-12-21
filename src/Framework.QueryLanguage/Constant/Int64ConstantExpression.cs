using System.Runtime.Serialization;

namespace Framework.QueryLanguage
{
    [DataContract]
    public class Int64ConstantExpression : ConstantExpression<long>
    {
        public Int64ConstantExpression(long value)
            : base(value)
        {

        }
    }
}