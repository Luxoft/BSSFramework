using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.Configuration.BLL;

public partial class SubscriptionBLL
{
    public IList<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo)
    {
        if (changedObjectInfo == null)
        {
            throw new ArgumentNullException(nameof(changedObjectInfo));
        }

        try
        {
            var domainType = this.Context.Logics.DomainType.GetByDomainType(changedObjectInfo.TypeInfo);

            if (!domainType.TargetSystem.IsRevision)
            {
                throw new InvalidOperationException($"{nameof(SubscriptionBLL)}::{nameof(this.Process)}: For {nameof(DomainType)} \'{domainType.Name}\' in {nameof(TargetSystem)} \'{domainType.TargetSystem.Name}\' {nameof(TargetSystem.IsRevision)} false but must be true.");
            }

            var subscriptionService = this.Context.GetTargetSystemService(domainType.TargetSystem).SubscriptionService;

            return subscriptionService.Process(changedObjectInfo);
        }
        catch (Exception ex)
        {
            return new[] { TryResult.CreateFault<Subscription>(ex) };
        }
    }

    public IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(object previous, object current, Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        try
        {
            var subscriptionService = this.Context.GetSubscriptionSystemService(type);

            return subscriptionService.ProcessChangedObjectUntyped(previous, current, type);
        }
        catch (Exception ex)
        {
            return new[] { TryResult.CreateFault<Subscription>(ex) };
        }
    }

    public SubscriptionRecipientInfo GetRecipientsUntyped(Type type, object previous, object current, string subscriptionCode)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        var subscriptionService = this.Context.GetSubscriptionSystemService(type);

        return subscriptionService.GetRecipientsUntyped(type, previous, current, subscriptionCode);
    }
}
