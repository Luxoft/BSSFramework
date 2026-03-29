#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.
using Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Domain;
using Framework.Subscriptions.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Recipients;

internal sealed class ByRolesRecipientsResolverNonContext<TBLLContext> : ByRolesRecipientsResolverBase<TBLLContext>
        where TBLLContext : class
{
    public ByRolesRecipientsResolverNonContext(
            ConfigurationContextFacade configurationContextFacade,
            LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
            : base(configurationContextFacade, lambdaProcessorFactory)
    {
    }

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
