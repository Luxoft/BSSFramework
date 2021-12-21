using System;

using Framework.Core;
using Framework.Configuration.Domain;
using Framework.Validation;

namespace Framework.Configuration.BLL
{
    public partial class SubscriptionLambdaBLL
    {
        public SubscriptionLambda Create(SubscriptionLambdaCreateModel createModel)
        {
            if (createModel == null) throw new ArgumentNullException(nameof(createModel));

            return new SubscriptionLambda();
        }

        public override void Save(SubscriptionLambda subscriptionLambda)
        {
            if (subscriptionLambda == null) throw new ArgumentNullException(nameof(subscriptionLambda));

            this.Context.Validator.Validate(subscriptionLambda, ConfigurationOperationContextC.PreSave);

            var subscriptionLambdaExpressionParser = this.Context.GetSubscriptionExpressionParser(subscriptionLambda);

            var del = subscriptionLambdaExpressionParser.GetDelegate(subscriptionLambda);

            subscriptionLambda.FormattedType = del.GetType().ToCSharpShortName();

            base.Save(subscriptionLambda);
        }
    }
}