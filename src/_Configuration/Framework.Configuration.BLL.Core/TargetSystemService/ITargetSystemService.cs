using Framework.Application.Domain;
using Framework.BLL.Domain.IdentityObject;
using Framework.Configuration.Domain;
using Framework.Core.TypeResolving;

namespace Framework.Configuration.BLL.TargetSystemService;

public interface ITargetSystemService : ITypeResolverContainer<DomainType>, ITargetSystemElement<TargetSystem>, IVisualIdentityObject
{
    IRevisionSubscriptionSystemService SubscriptionService { get; }

    ITypeResolver<string> TypeResolverS { get; }

    bool IsAssignable(Type domainType);

    void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId);
}
