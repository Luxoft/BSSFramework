using System.Runtime.Serialization;

namespace Framework.QueryLanguage
{
    [DataContract]
    public class Int32ConstantExpression : ConstantExpression<int>
    {
        public Int32ConstantExpression(int value)
            : base (value)
        {

        }
    }
}