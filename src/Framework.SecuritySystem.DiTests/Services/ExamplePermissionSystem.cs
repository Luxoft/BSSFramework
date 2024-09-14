using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.ExternalSystem.Management;

namespace Framework.SecuritySystem.DiTests;

public class ExamplePermissionSystem (ExamplePermissionSystemData data) : IPermissionSystem
{
    public Type PermissionType => throw new NotImplementedException();

    public IPrincipalService PrincipalService => throw new NotImplementedException();

    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule, bool withRunAs) => new ExamplePermissionSource(data);

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
