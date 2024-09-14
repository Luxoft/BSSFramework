using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.Expanders;

public class RoleFactorySecurityRuleExpander(IServiceProvider serviceProvider)
{
    public DomainSecurityRule.RoleBaseSecurityRule Expand(DomainSecurityRule.RoleFactorySecurityRule securityRule)
    {
        var factory = (IFactory<DomainSecurityRule.RoleBaseSecurityRule>)serviceProvider.GetRequiredService(securityRule.RoleFactoryType);

        var resultRule = factory.Create();

        return securityRule.CustomExpandType == null
                   ? resultRule
                   : resultRule with { CustomExpandType = securityRule.CustomExpandType, CustomCredential = securityRule.CustomCredential };
    }
}
