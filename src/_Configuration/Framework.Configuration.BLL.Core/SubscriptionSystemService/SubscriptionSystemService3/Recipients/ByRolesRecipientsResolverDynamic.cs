#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.
using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL.SubscriptionSystemService3.Lambdas;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

internal sealed class ByRolesRecipientsResolverDynamic<TBLLContext> : ByRolesRecipientsResolverBase<TBLLContext>
        where TBLLContext : class
{
    public ByRolesRecipientsResolverDynamic(
            ConfigurationContextFacade configurationContextFacade,
            LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
            : base(configurationContextFacade, lambdaProcessorFactory)
    {
    }

    internal override RecipientCollection Resolve<T>(Subscription subscription, DomainObjectVersions<T> versions)
    {
        if (subscription.SourceMode != SubscriptionSourceMode.Dynamic)
        {
            return new RecipientCollection();
        }

        var businessRolesIds = this.GetBusinessRoles(subscription);
        var fids = this.GetFilterItemIdentities(subscription, versions).ToList();
        var filterGroups = this.GetNotificationFilterGroups(fids, subscription.DynamicSourceExpandType.Value);

        var principals = this.ConfigurationContextFacade.GetNotificationPrincipals(businessRolesIds, filterGroups);
        var employees = this.ConfigurationContextFacade.ConvertPrincipals(principals);
        var recipients = employees.Select(this.CreateRecipient);

        return new RecipientCollection(recipients);
    }

    private IEnumerable<FilterItemIdentity> GetFilterItemIdentities<T>(
            Subscription subscription,
            DomainObjectVersions<T> versions)
            where T : class
    {
        var processor = this.LambdaProcessorFactory.Create<DynamicSourceLambdaProcessor<TBLLContext>>();
        var result = processor.Invoke(subscription, versions);

        return result;
    }

    private IEnumerable<NotificationFilterGroup> GetNotificationFilterGroups(
            IEnumerable<FilterItemIdentity> fids,
            NotificationExpandType expandType)
    {
        var result =
                from item in fids.GroupBy(fid => fid.Type)
                let securityContextType = item.Key
                let ids = item.Select(i => i.Id)
                let et = securityContextType.IsHierarchical() ? expandType : expandType.WithoutHierarchical()
                select new NotificationFilterGroup(securityContextType, ids, et);

        return result;
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
