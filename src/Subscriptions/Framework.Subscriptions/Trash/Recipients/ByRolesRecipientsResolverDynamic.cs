#pragma warning disable SA1600 // ElementsMustBeDocumented. Internal type does not require inline documentation by convention.

using System.Collections.Immutable;

using Framework.Subscriptions.Domain;

using HierarchicalExpand;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Notification.Domain;

namespace Framework.Subscriptions.Recipients;

internal sealed class ByRolesRecipientsResolverDynamic<TBLLContext>(
    ConfigurationContextFacade configurationContextFacade,
    LambdaProcessorFactory<TBLLContext> lambdaProcessorFactory)
    : ByRolesRecipientsResolverBase<TBLLContext>(configurationContextFacade, lambdaProcessorFactory)
    where TBLLContext : class
{
    internal override ImmutableArray<IEmployee> Resolve<T>(Subscription subscription, DomainObjectVersions<T> versions)
    {
        if (subscription.SourceMode != SubscriptionSourceMode.Dynamic)
        {
            return new ImmutableArray<IEmployee>();
        }

        var businessRolesIds = this.GetBusinessRoles(subscription);
        var fids = this.GetFilterItemIdentities(subscription, versions).ToList();
        var filterGroups = this.GetNotificationFilterGroups(fids, subscription.DynamicSourceExpandType.Value);

        var principals = this.ConfigurationContextFacade.GetNotificationPrincipals(businessRolesIds, filterGroups);
        var employees = this.ConfigurationContextFacade.ConvertPrincipals(principals);
        var recipients = employees.Select(this.CreateRecipient);

        return new ImmutableArray<IEmployee>(recipients);
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

    private ImmutableArray<NotificationFilterGroup> GetNotificationFilterGroups(
        IEnumerable<FilterItemIdentity> fids,
        NotificationExpandType expandType)
    {
        var result =
            from item in fids.GroupBy(fid => fid.Type)
            let securityContextType = item.Key
            let ids = item.Select(i => i.Id)
            let et = this.ConfigurationContextFacade.ServiceProvider.GetRequiredService<IHierarchicalInfoSource>().IsHierarchical(securityContextType)
                         ? expandType
                         : expandType.WithoutHierarchical()
            select (NotificationFilterGroup)new NotificationFilterGroup<Guid> { Idents = [..ids], SecurityContextType = securityContextType, ExpandType = et };

        return [.. result];
    }
}
#pragma warning restore SA1600 // ElementsMustBeDocumented
