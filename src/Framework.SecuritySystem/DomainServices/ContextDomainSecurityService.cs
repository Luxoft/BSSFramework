namespace Framework.SecuritySystem;

/// <summary>
/// Сервис с кешированием доступа к контекстным операциям
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public abstract class ContextDomainSecurityServiceBase<TDomainObject>(
    IServiceProvider serviceProvider,
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityPathProviderFactory securityPathProviderFactory)
    : DomainSecurityService<TDomainObject>(serviceProvider, disabledSecurityProvider, securityRuleExpander)
{
    protected virtual ISecurityProvider<TDomainObject> Create(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.DomainObjectSecurityRule securityRule)
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityRule == null) throw new ArgumentNullException(nameof(securityRule));

        return securityPathProviderFactory.Create(securityPath, securityRule);
    }
}

public class ContextDomainSecurityService<TDomainObject>(
    IServiceProvider serviceProvider,
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityPathProviderFactory securityPathProviderFactory,
    SecurityPath<TDomainObject> securityPath)
    : ContextDomainSecurityServiceBase<TDomainObject>(
        serviceProvider,
        disabledSecurityProvider,
        securityRuleExpander,
        securityPathProviderFactory)
{
    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        return this.Create(securityPath, securityRule);
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.CompositeSecurityRule securityRule)
    {
        return this.Create(securityPath, securityRule);
    }
}
