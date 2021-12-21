using System.Runtime.Serialization;

namespace Framework.QueryLanguage
{
    [DataContract]
    public class StringConstantExpression : ConstantExpression<string>
    {
        public StringConstantExpression(string value)
            : base(value)
        {

        }

        public override string ToString()
        {
            return $"\"{this.Value}\"";
        }
    }
}