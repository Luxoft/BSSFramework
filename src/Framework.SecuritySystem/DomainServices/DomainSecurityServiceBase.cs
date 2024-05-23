using Framework.Core;

namespace Framework.SecuritySystem;

public abstract class DomainSecurityServiceBase<TDomainObject> : IDomainSecurityService<TDomainObject>
{
    private readonly IDictionaryCache<SecurityRule, ISecurityProvider<TDomainObject>> providersCache;

    protected DomainSecurityServiceBase(ISecurityProvider<TDomainObject> disabledSecurityProvider)
    {
        this.providersCache = new DictionaryCache<SecurityRule, ISecurityProvider<TDomainObject>>(securityRule =>
        {
            if (securityRule == SecurityRule.Disabled)
            {
                return disabledSecurityProvider;
            }
            else
            {
                return this.CreateSecurityProvider(securityRule)
                           .ApplySourceRuleDeclaration(securityRule);
            }
        }).WithLock();
    }

    protected abstract ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule);

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityRule securityRule)
    {
        return this.providersCache[securityRule];
    }
}
