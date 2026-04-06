using System.Collections.Immutable;
using System.Security.Principal;

using Framework.Core;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using SecuritySystem.Notification.Domain;
using SecuritySystem.UserSource;

namespace Framework.Subscriptions;

public class SubscriptionService(IEnumerable<ISubscription> subscriptionMetadataList) : ISubscriptionService
{
    public IEnumerable<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions versions)
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }

    private IEnumerable<ITryResult<SubscriptionHeader>> Process<TDomainObject>(DomainObjectVersions<TDomainObject> versions)
        where TDomainObject : class
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }
}

public class SubscriptionService<TDomainObject, TRenderingObject>(
    IServiceProvider serviceProvider,
    ISubscription<TDomainObject, TRenderingObject> subscription,
    IEmployeeEmailExtractor employeeEmailExtractor)
    where TDomainObject : class
    where TRenderingObject : class
{
    public IEnumerable<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions<TDomainObject> versions)
    {
        if (subscription.IsProcessed(serviceProvider, versions))
        {
            var to = subscription.GetTo(serviceProvider, versions);

            var authTo = this.GetAuthTo(versions);

            var copyTo = subscription.GetCopyTo(serviceProvider, versions);
            var replyTo = subscription.GetReplyTo(serviceProvider, versions);


            yield return TryResult.Return(subscription.Header);
        }
    }

    private IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetAuthTo(DomainObjectVersions<TDomainObject> versions)
    {
        if (subscription.SecurityRoles.Length > 0)
        {
            var notificationFilterGroups = subscription.GetNotificationFilterGroups(serviceProvider, versions).ToImmutableArray();

            if (notificationFilterGroups.Length > 0)
            {
                var emails = employeeEmailExtractor.GetEmails(subscription.SecurityRoles, notificationFilterGroups);

                if (emails.Count > 0)
                {
                    var renderingVersions = versions.ChangeDomainObject(domainObject => subscription.ConvertToRenderingObject(serviceProvider, domainObject));

                    yield return new NotificationMessageGenerationInfo<TRenderingObject>(emails, renderingVersions);
                }
            }
        }
    }
}
