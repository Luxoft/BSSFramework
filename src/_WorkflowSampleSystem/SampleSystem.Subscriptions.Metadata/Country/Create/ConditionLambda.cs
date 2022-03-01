using Framework.Configuration.SubscriptionModeling;

namespace SampleSystem.Subscriptions.Metadata.Country.Create
{
    /// <inheritdoc />
    public sealed class ConditionLambda : ConditionLambdaBase<Domain.Country>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionLambda"/> class.
        /// </summary>
        public ConditionLambda()
        {
            this.DomainObjectChangeType = DomainObjectChangeType.Create;
            this.Lambda = (context, versions) => true;
        }
    }
}
