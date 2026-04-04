using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Database;
using Framework.Database.Domain;

namespace Framework.Configuration.BLL;

public interface IRevisionSubscriptionSystemService : ISubscriptionSystemService
{
    List<ITryResult<Subscription>> Process(ObjectModificationInfo<Guid> changedObjectInfo);

    IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes);
}
