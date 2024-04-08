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
        return this.GetSecurityProviderInternal(securityRule, securityRule == SecurityRule.Disabled);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityRule securityRule)
    {
        return this.GetSecurityProviderInternal(securityRule, securityRule == SecurityRule.Disabled);
    }

    private ISecurityProvider<TDomainObject> GetSecurityProviderInternal<TSecurityMode>(TSecurityMode securityRule, bool isDisabled)
    {
        if (isDisabled)
        {
            return this.disabledSecurityProvider;
        }
        else
        {
            throw new InvalidOperationException($"Security mode \"{securityRule}\" not allowed");
        }
    }
}
