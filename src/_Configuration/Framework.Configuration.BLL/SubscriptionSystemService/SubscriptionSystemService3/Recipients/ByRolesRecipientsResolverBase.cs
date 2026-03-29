#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.

using System.Collections.Immutable;

using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Domain;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;

using SecuritySystem;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

internal class ByRolesRecipientsResolverBase<TBLLContext>
        where TBLLContext : class
{
    public ByRolesRecipientsResolverBase(
            ConfigurationContextFacade configurationContextFacade,
            LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
    {
        if (configurationContextFacade == null)
        {
            throw new ArgumentNullException(nameof(configurationContextFacade));
        }

        if (lambdaProcessorFactory == null)
        {
            throw new ArgumentNullException(nameof(lambdaProcessorFactory));
        }

        this.ConfigurationContextFacade = configurationContextFacade;
        this.LambdaProcessorFactory = lambdaProcessorFactory;
    }

    protected ConfigurationContextFacade ConfigurationContextFacade { get; }

    protected LambdaProcessorFactory<TBLLContext> LambdaProcessorFactory { get; }

    internal virtual RecipientCollection Resolve<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        return new RecipientCollection();
    }

    protected Recipient CreateRecipient(IEmployee employee)
    {
        return new Recipient(employee.Login, employee.Email);
    }

    protected ImmutableArray<SecurityRole> GetBusinessRoles(Subscription subscription)
    {
        var result = subscription.SubBusinessRoles.Select(r => r.SecurityRole).ToImmutableArray();

        return result;
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
