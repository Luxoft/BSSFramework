using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public class LocalStorage<TSecurityContext>
    where TSecurityContext : IIdentityObject<Guid>, ISecurityContext
{
    private readonly HashSet<TSecurityContext> items = new();

    public bool IsExists(Guid securityEntityId)
    {
        return this.items.Select(v => v.Id).Contains(securityEntityId);
    }

    public bool Register(TSecurityContext securityContext)
    {
        return this.items.Add(securityContext);
    }
}
