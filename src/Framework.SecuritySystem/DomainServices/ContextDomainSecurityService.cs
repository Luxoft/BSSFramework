namespace Framework.SecuritySystem;

/// <summary>
/// Сервис с кешированием доступа к контекстным операциям
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public abstract class ContextDomainSecurityServiceBase<TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityPathProviderFactory securityPathProviderFactory)
    : DomainSecurityService<TDomainObject>(securityRuleExpander)
{
    protected virtual ISecurityProvider<TDomainObject> Create(SecurityPath<TDomainObject> securityPath, SecurityRule.DomainObjectSecurityRule securityRule)
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityRule == null) throw new ArgumentNullException(nameof(securityRule));

        return securityPathProviderFactory.Create(securityPath, securityRule);
    }
}

public class ContextDomainSecurityService<TDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityPathProviderFactory securityPathProviderFactory,
    SecurityPath<TDomainObject> securityPath)
    : ContextDomainSecurityServiceBase<TDomainObject>(
        securityRuleExpander,
        securityPathProviderFactory)
{
    protected override ISecurityProvider<TDomainObject> CreateFinalSecurityProvider(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return this.Create(securityPath, securityRule);
    }
}
