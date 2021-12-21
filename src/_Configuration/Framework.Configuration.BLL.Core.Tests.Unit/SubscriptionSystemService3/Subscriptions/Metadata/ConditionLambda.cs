namespace Framework.Configuration.BLL.Core.Tests.Unit.SubscriptionSystemService3.Subscriptions.Metadata
{
    internal sealed class ConditionLambda : LambdaMetadataBase<object, bool>
    {
        public ConditionLambda()
        {
            this.Lambda = (context, versions) => true;
        }
    }
}
