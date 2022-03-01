using System.Reflection;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using WorkflowSampleSystem.Subscriptions.Metadata.Employee.Update;

namespace WorkflowSampleSystem.ServiceEnvironment
{
    public sealed class WorkflowSampleSystemSubscriptionsMetadataFinder : SubscriptionMetadataFinder
    {
        protected override Assembly[] GetSubscriptionMetadataAssemblies()
        {
            return new[] { typeof(EmployeeUpdateSubscription).Assembly };
        }
    }
}
