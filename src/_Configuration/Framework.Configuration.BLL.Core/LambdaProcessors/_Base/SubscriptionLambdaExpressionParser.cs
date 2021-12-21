using Framework.ExpressionParsers;
using Framework.DomainDriven.BLL;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public class SubscriptionLambdaExpressionParser<TDelegate> : LambdaObjectExpressionParser<SubscriptionLambda, TDelegate>
        where TDelegate : class
    {
        public SubscriptionLambdaExpressionParser(INativeExpressionParser parser)
            : base(parser)
        {

        }
    }
}