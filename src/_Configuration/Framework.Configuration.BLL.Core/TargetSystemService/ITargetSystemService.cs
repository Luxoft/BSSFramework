using Framework.Configuration.Domain;
using Framework.Database;
using Framework.Database.Domain;

namespace Framework.Configuration.BLL.TargetSystemService;

public interface ITargetSystemService
{
    TargetSystem TargetSystem { get; }

    bool IsAssignable(Type domainType);

    void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId);

    IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes);
}
