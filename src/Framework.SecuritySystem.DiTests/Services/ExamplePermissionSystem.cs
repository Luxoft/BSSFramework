using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExamplePermissionSystem(ExamplePermissionSystemData data) : IPermissionSystem
{
    public Type PermissionType => throw new NotImplementedException();

    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => new ExamplePermissionSource(data);

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
