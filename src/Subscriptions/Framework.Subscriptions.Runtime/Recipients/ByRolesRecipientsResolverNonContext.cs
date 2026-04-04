#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Lambdas;

namespace Framework.Subscriptions.Recipients;

internal sealed class ByRolesRecipientsResolverNonContext<TBLLContext>(
    ConfigurationContextFacade configurationContextFacade,
    LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
    : ByRolesRecipientsResolverBase<TBLLContext>(configurationContextFacade, lambdaProcessorFactory)
    where TBLLContext : class
{
    internal override RecipientCollection Resolve<T>(Subscription subscription, DomainObjectVersions<T> versions)
    {
        if (subscription.SourceMode != SubscriptionSourceMode.NonContext)
        {
            return new RecipientCollection();
        }

        var principals = this.ConfigurationContextFacade.GetNotificationPrincipals(this.GetBusinessRoles(subscription));
        var recipients = this.ConfigurationContextFacade.ConvertPrincipals(principals).Select(this.CreateRecipient);

        return new RecipientCollection(recipients);
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
