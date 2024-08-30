using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem;

public class ContextDomainSecurityService<TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    IDomainSecurityProviderFactory<TDomainObject> domainSecurityProviderFactory,
    SecurityPath<TDomainObject>? securityPath = null)
    : DomainSecurityService<TDomainObject>(securityRuleExpander)
{
    protected virtual ISecurityProvider<TDomainObject> Create(
        DomainSecurityRule securityRule,
        SecurityPath<TDomainObject> customSecurityPath) => domainSecurityProviderFactory.Create(securityRule, customSecurityPath);

    protected override ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(DomainSecurityRule securityRule) =>
        this.Create(securityRule, securityPath ?? SecurityPath<TDomainObject>.Empty);
}
