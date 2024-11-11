using Framework.SecuritySystem.Services;

namespace Framework.SecuritySystem;

public class DomainSecurityService<TDomainObject>(IDomainSecurityProviderFactory<TDomainObject> domainSecurityProviderFactory)
    : DomainSecurityServiceBase<TDomainObject>
{
    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(
        SecurityRule securityRule,
        SecurityPath<TDomainObject>? customSecurityPath) =>
        domainSecurityProviderFactory.Create(securityRule, customSecurityPath);

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule securityRule) =>
        this.CreateSecurityProvider(securityRule, null);
}
