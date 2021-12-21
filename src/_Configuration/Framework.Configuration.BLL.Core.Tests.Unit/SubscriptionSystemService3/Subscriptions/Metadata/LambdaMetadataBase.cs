using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata
{
    internal abstract class LambdaMetadataBase<TDomainObject, TResult>
        : LambdaMetadata<object, TDomainObject, TResult>,
            ILambdaMetadataBase
        where TDomainObject : class
    {
        private DomainObjectChangeType domainObjectChangeType =
            DomainObjectChangeType.Update;

        public override DomainObjectChangeType DomainObjectChangeType
            => this.domainObjectChangeType;

        public void SetDomainObjectChangeType(DomainObjectChangeType changeType)
        {
            this.domainObjectChangeType = changeType;
        }
    }
}