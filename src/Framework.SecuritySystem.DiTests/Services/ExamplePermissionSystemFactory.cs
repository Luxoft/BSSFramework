using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.DiTests;

public class ExamplePermissionSystemFactory(ISecurityRuleExpander securityRuleExpander, TestPermissionData data) : IPermissionSystemFactory
{
    public IPermissionSystem Create(SecurityRuleCredential securityRuleCredential) =>
        new ExamplePermissionSystem(securityRuleExpander, data);
}
