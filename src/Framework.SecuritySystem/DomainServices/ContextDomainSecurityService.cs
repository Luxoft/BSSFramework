using Framework.SecuritySystem.Expanders;
using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem;

public class ContextDomainSecurityService<TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    IDomainSecurityProviderFactory domainSecurityProviderFactory,
    SecurityPath<TDomainObject>? securityPath = null)
    : DomainSecurityService<TDomainObject>(securityRuleExpander)
{
    protected virtual ISecurityProvider<TDomainObject> Create(
        SecurityPath<TDomainObject> customSecurityPath,
        DomainSecurityRule securityRule) => domainSecurityProviderFactory.Create(customSecurityPath, securityRule);

    protected override ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(DomainSecurityRule securityRule) =>
        this.Create(securityPath ?? SecurityPath<TDomainObject>.Empty, securityRule);
}
