#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.
using System.Collections.Immutable;
using System.Reflection;

using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Persistent;

using SecuritySystem.Notification;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

internal sealed class ByRolesRecipientsResolverTyped<TBLLContext> : ByRolesRecipientsResolverBase<TBLLContext>
        where TBLLContext : class
{
    public ByRolesRecipientsResolverTyped(
            ConfigurationContextFacade configurationContextFacade,
            LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
            : base(configurationContextFacade, lambdaProcessorFactory)
    {
    }

    internal override RecipientCollection Resolve<T>(Subscription subscription, DomainObjectVersions<T> versions)
    {
        var businessRolesIds = this.GetBusinessRoles(subscription);
        var filterGroups = this.GetNotificationFilterGroups(subscription, versions);
        var principals = this.ConfigurationContextFacade.GetNotificationPrincipals(businessRolesIds, filterGroups);
        var employees = this.ConfigurationContextFacade.ConvertPrincipals(principals);
        var recipients = employees.Select(this.CreateRecipient);

        return new RecipientCollection(recipients);
    }

    private ImmutableArray<NotificationFilterGroup> GetNotificationFilterGroups<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        return [
            ..subscription
              .SecurityItems
              .Select(securityItem => this.GetNotificationFilterGroup(versions, securityItem))
        ];
    }

    private NotificationFilterGroup GetNotificationFilterGroup<T>(
            DomainObjectVersions<T> versions,
            SubscriptionSecurityItem securityItem)
            where T : class
    {
        var securityItemType = this.GetSecurityItemType(securityItem);

        var method = this
                     .GetType()
                     .GetMethod(nameof(this.GetNotificationFilterGroupTyped), BindingFlags.Instance | BindingFlags.NonPublic)!
                     .MakeGenericMethod(typeof(T), securityItemType);

        var notificationFilterGroup = (NotificationFilterGroup)method.Invoke(
                                                                             this,
                                                                             new object[] { securityItem, versions })!;

        return notificationFilterGroup;
    }

    private NotificationFilterGroup GetNotificationFilterGroupTyped<T, TSecurityItem>(
            SubscriptionSecurityItem securityItem,
            DomainObjectVersions<T> versions)
            where T : class
            where TSecurityItem : IIdentityObject<Guid>
    {
        var lambdaProcessor = this.LambdaProcessorFactory.Create<SecurityItemSourceLambdaProcessor<TBLLContext>>();
        var identityObjects = lambdaProcessor.Invoke<T, TSecurityItem>(securityItem, versions);
        var result = new NotificationFilterGroup<Guid>
                     {
                         Idents = [..identityObjects.Select(v => v.Id)],
                         ExpandType = securityItem.ExpandType,
                         SecurityContextType = typeof(TSecurityItem)
                     };

        return result;
    }

    private Type GetSecurityItemType(SubscriptionSecurityItem securityItem)
    {
        var result = securityItem.Source.AuthDomainType;

        return result;
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
