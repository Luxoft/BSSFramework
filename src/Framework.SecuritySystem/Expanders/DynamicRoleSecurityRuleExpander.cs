using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.Expanders;

public class DynamicRoleSecurityRuleExpander(IServiceProvider serviceProvider)
{
    public SecurityRule.RoleBaseSecurityRule Expand(SecurityRule.DynamicRoleSecurityRule securityRule)
    {
        var factory = (IFactory<SecurityRule.RoleBaseSecurityRule>)serviceProvider.GetRequiredService(securityRule.DynamicRoleFactoryType);

        var resultRule = factory.Create();

        return securityRule.CustomExpandType == null ? resultRule : resultRule with { CustomExpandType = securityRule.CustomExpandType };
    }
}
