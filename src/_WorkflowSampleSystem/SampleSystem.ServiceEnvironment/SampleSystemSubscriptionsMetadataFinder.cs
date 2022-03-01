using System.Reflection;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using SampleSystem.Subscriptions.Metadata.Employee.Update;

namespace SampleSystem.ServiceEnvironment
{
    public sealed class SampleSystemSubscriptionsMetadataFinder : SubscriptionMetadataFinder
    {
        protected override Assembly[] GetSubscriptionMetadataAssemblies()
        {
            return new[] { typeof(EmployeeUpdateSubscription).Assembly };
        }
    }
}
