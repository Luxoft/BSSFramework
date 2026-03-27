using Framework.Core;
using Framework.Persistent;
using Framework.Configuration.Domain;
using Framework.Core.TypeResolving;

namespace Framework.Configuration.BLL;

public interface ITargetSystemService : ITypeResolverContainer<DomainType>, ITargetSystemElement<TargetSystem>, IVisualIdentityObject
{
    IRevisionSubscriptionSystemService SubscriptionService { get; }

    ITypeResolver<string> TypeResolverS { get; }

    bool IsAssignable(Type domainType);

    void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId);
}
