using Framework.Application.Domain;
using Framework.BLL.Domain.IdentityObject;
using Framework.Configuration.Domain;
using Framework.Core.TypeResolving;
using Framework.Database;
using Framework.Database.Domain;

namespace Framework.Configuration.BLL.TargetSystemService;

public interface ITargetSystemService : ITypeResolverContainer<DomainType>, ITargetSystemElement<TargetSystem>, IVisualIdentityObject
{
    Type PersistentDomainObjectBaseType { get; }

    Type BLLContextType { get; }

    ITypeResolver<string> TypeResolverS { get; }

    bool IsAssignable(Type domainType);

    void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId);

    IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes);
}
