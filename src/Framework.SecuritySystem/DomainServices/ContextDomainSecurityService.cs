namespace Framework.SecuritySystem;

public class ContextDomainSecurityService<TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    IDomainSecurityProviderFactory domainSecurityProviderFactory,
    SecurityPath<TDomainObject> securityPath = null)
    : DomainSecurityService<TDomainObject>(securityRuleExpander)
{
    protected virtual ISecurityProvider<TDomainObject> Create(SecurityPath<TDomainObject> customSecurityPath, SecurityRule.DomainSecurityRule securityRule)
    {
        if (customSecurityPath == null) throw new ArgumentNullException(nameof(customSecurityPath));
        if (securityRule == null) throw new ArgumentNullException(nameof(securityRule));

        return domainSecurityProviderFactory.Create(customSecurityPath, securityRule);
    }

    protected override ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(SecurityRule.DomainSecurityRule securityRule)
    {
        return this.Create(securityPath ?? SecurityPath<TDomainObject>.Empty, securityRule);
    }
}
