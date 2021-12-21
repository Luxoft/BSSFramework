using System.Runtime.Serialization;

namespace Framework.QueryLanguage
{
    [DataContract]
    public class DecimalConstantExpression : ConstantExpression<decimal>
    {
        public DecimalConstantExpression(decimal value)
            : base(value)
        {

        }
    }
}