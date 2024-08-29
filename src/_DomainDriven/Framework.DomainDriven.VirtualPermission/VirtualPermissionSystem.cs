using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystem<TDomainObject>(VirtualPermissionBindingInfo<TDomainObject> bindingInfo) : IPermissionSystem
{
    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule) => throw new NotImplementedException();

    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => throw new NotImplementedException();

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
