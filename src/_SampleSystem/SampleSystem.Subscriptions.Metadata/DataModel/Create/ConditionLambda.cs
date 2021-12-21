using Framework.Configuration.SubscriptionModeling;

using SampleSystem.Domain.Models.Custom;

namespace SampleSystem.Subscriptions.Metadata.DataModel.Create
{
    public sealed class ConditionLambda : ConditionLambdaBase<DateModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleSystem.Subscriptions.Metadata.Employee.Update.ConditionLambda"/> class.
        /// </summary>
        public ConditionLambda()
        {
            this.DomainObjectChangeType = DomainObjectChangeType.Create;
            this.Lambda = (context, versions) => true;
        }
    }
}
