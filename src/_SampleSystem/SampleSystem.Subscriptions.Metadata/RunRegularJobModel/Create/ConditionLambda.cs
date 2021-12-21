using Framework.Configuration.BLL;
using Framework.Configuration.SubscriptionModeling;

namespace SampleSystem.Subscriptions.Metadata.RunRegularJobModel.Create
{
    public sealed class ConditionLambda : LambdaMetadata<IConfigurationBLLContext, Framework.Configuration.Domain.RunRegularJobModel, bool>
    {
        public ConditionLambda()
        {
            this.DomainObjectChangeType = DomainObjectChangeType.Create;
            this.Lambda = (context, versions) => true;
        }
    }
}
