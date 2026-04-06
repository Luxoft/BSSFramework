#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.
using System.Collections.Immutable;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Lambdas;

namespace Framework.Subscriptions.Recipients;

internal sealed class ByRolesRecipientsResolverNonContext<TBLLContext>(
    ConfigurationContextFacade configurationContextFacade,
    LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
    : ByRolesRecipientsResolverBase<TBLLContext>(configurationContextFacade, lambdaProcessorFactory)
    where TBLLContext : class
{
    internal override ImmutableArray<IEmployee> Resolve<T>(Subscription subscription, DomainObjectVersions<T> versions)
    {
        if (subscription.SourceMode != SubscriptionSourceMode.NonContext)
        {
            return new ImmutableArray<IEmployee>();
        }

        var principals = this.ConfigurationContextFacade.GetNotificationPrincipals(this.GetBusinessRoles(subscription));
        var recipients = this.ConfigurationContextFacade.ConvertPrincipals(principals).Select(this.CreateRecipient);

        return new ImmutableArray<IEmployee>(recipients);
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
