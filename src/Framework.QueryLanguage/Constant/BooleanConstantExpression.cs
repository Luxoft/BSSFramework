using System.Runtime.Serialization;

namespace Framework.QueryLanguage
{
    [DataContract]
    public class BooleanConstantExpression : ConstantExpression<bool>
    {
        public BooleanConstantExpression(bool value)
            : base(value)
        {

        }


        public static readonly BooleanConstantExpression True = new BooleanConstantExpression(true);

        public static readonly BooleanConstantExpression False = new BooleanConstantExpression(false);
    }
}
