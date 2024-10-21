using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.Expanders;

public class RoleFactorySecurityRuleExpander(IServiceProvider serviceProvider) : IRoleFactorySecurityRuleExpander
{
    public DomainSecurityRule.RoleBaseSecurityRule Expand(DomainSecurityRule.RoleFactorySecurityRule securityRule)
    {
        var factory = (IFactory<DomainSecurityRule.RoleBaseSecurityRule>)serviceProvider.GetRequiredService(securityRule.RoleFactoryType);

        return factory.Create().WithCopyCustoms(securityRule);
    }
}
