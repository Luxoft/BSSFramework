using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;

public class OnlyDisabledDomainSecurityService<TDomainObject> : IDomainSecurityService<TDomainObject>
{
    private readonly ISecurityProvider<TDomainObject> disabledSecurityProvider;

    public OnlyDisabledDomainSecurityService(ISecurityProvider<TDomainObject> disabledSecurityProvider)
    {
        this.disabledSecurityProvider = disabledSecurityProvider;
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityRule securityRule)
    {
        if (securityRule == SecurityRule.Disabled)
        {
            return this.disabledSecurityProvider;
        }
        else
        {
            throw new InvalidOperationException($"Security mode \"{securityRule}\" not allowed");
        }
    }
}
