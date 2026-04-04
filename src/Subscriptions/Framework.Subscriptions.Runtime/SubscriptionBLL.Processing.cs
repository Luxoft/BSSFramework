using Framework.Core;

namespace Framework.Subscriptions;

public partial class SubscriptionBLL
{
    public List<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo)
    {
        if (changedObjectInfo == null)
        {
            throw new ArgumentNullException(nameof(changedObjectInfo));
        }

        try
        {
            var domainType = context.Logics.DomainType.GetByDomainType(new MemoryDomainType(changedObjectInfo.TypeInfo.Name, changedObjectInfo.TypeInfo.NameSpace));

            if (!domainType.TargetSystem.IsRevision)
            {
                throw new InvalidOperationException(
                    $"{nameof(SubscriptionBLL)}::{nameof(this.Process)}: For {nameof(DomainType)} \'{domainType.Name}\' in {nameof(TargetSystem)} \'{domainType.TargetSystem.Name}\' {nameof(TargetSystem.IsRevision)} false but must be true.");
            }

            var subscriptionService = context.GetTargetSystemService(domainType.TargetSystem).SubscriptionService;

            return subscriptionService.Process(changedObjectInfo);
        }
        catch (Exception ex)
        {
            return [TryResult.CreateFault<Subscription>(ex)];
        }
    }

    public List<ITryResult<Subscription>> ProcessChangedObjectUntyped(object previous, object current, Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        try
        {
            var subscriptionService = context.GetSubscriptionSystemService(type);

            return subscriptionService.ProcessChangedObjectUntyped(previous, current, type);
        }
        catch (Exception ex)
        {
            return [TryResult.CreateFault<Subscription>(ex)];
        }
    }

    public SubscriptionRecipientInfo GetRecipientsUntyped(Type type, object previous, object current, string subscriptionCode)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var subscriptionService = context.GetSubscriptionSystemService(type);

        return subscriptionService.GetRecipientsUntyped(type, previous, current, subscriptionCode);
    }
}
