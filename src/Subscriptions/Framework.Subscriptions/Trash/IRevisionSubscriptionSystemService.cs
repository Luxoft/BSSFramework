using Framework.Core;

namespace Framework.Subscriptions;

public interface IRevisionSubscriptionSystemService : ISubscriptionSystemService
{
    List<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo);

    IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes);
}
