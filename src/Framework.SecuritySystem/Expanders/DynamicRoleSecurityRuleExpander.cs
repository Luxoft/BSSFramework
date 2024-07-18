using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.Expanders;

public class DynamicRoleSecurityRuleExpander(IServiceProvider serviceProvider)
{
    public SecurityRule.RoleBaseSecurityRule Expand(SecurityRule.DynamicRoleSecurityRule securityRule)
    {
        var factory = (IFactory<SecurityRule.RoleBaseSecurityRule>)serviceProvider.GetRequiredService(securityRule.DynamicRoleFactoryType);

        return factory.Create();
    }
}
