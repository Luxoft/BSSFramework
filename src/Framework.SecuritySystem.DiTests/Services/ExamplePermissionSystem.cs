using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExamplePermissionSystem(ISecurityRuleExpander securityRuleExpander, TestPermissionData data) : IPermissionSystem
{
    public Type PermissionType => throw new NotImplementedException();

    public IPermissionSource GetPermissionSource(DomainSecurityRule.RoleBaseSecurityRule securityRule) => new ExamplePermissionSource(data, securityRuleExpander.FullRoleExpand(securityRule));

    public Task<IEnumerable<SecurityRole>> GetAvailableSecurityRoles(CancellationToken cancellationToken = default) => throw new NotImplementedException();
}
