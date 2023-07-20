#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.
using System.Reflection;

using Framework.Authorization.BLL;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Persistent;

using JetBrains.Annotations;

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
        var businessRolesIds = this.GetBusinessRolesIds(subscription);
        var filterGroups = this.GetNotificationFilterGroups(subscription, versions);
        var principals = this.ConfigurationContextFacade.GetNotificationPrincipals(businessRolesIds, filterGroups);
        var employees = this.ConfigurationContextFacade.ConvertPrincipals(principals);
        var recipients = employees.Select(this.CreateRecipient);

        return new RecipientCollection(recipients);
    }

    private IEnumerable<NotificationFilterGroup> GetNotificationFilterGroups<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        return subscription
               .SecurityItems
               .Select(securityItem => this.GetNotificationFilterGroup(versions, securityItem))
               .ToList();
    }

    private NotificationFilterGroup GetNotificationFilterGroup<T>(
            DomainObjectVersions<T> versions,
            SubscriptionSecurityItem securityItem)
            where T : class
    {
        var securityItemType = this.GetSecurityItemType(securityItem);

        var method = this
                     .GetType()
                     .GetMethod(nameof(this.GetNotificationFilterGroupTyped), BindingFlags.Instance | BindingFlags.NonPublic)
                     .MakeGenericMethod(typeof(T), securityItemType);

        var notificationFilterGroup = (NotificationFilterGroup)method.Invoke(
                                                                             this,
                                                                             new object[] { securityItem, versions });

        return notificationFilterGroup;
    }

    [UsedImplicitly]
    private NotificationFilterGroup GetNotificationFilterGroupTyped<T, TSecurityItem>(
            SubscriptionSecurityItem securityItem,
            DomainObjectVersions<T> versions)
            where T : class
            where TSecurityItem : IIdentityObject<Guid>
    {
        var lambdaProcessor = this.LambdaProcessorFactory.Create<SecurityItemSourceLambdaProcessor<TBLLContext>>();
        var identityObjects = lambdaProcessor.Invoke<T, TSecurityItem>(securityItem, versions);
        var result = new NotificationFilterGroup<TSecurityItem>(identityObjects, securityItem.ExpandType);

        return result;
    }

    private Type GetSecurityItemType(SubscriptionSecurityItem securityItem)
    {
        var result = securityItem.Source.AuthDomainType;

        return result;
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
