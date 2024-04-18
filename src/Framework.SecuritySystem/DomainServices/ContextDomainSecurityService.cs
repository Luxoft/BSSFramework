using System.Linq.Expressions;

using Framework.SecuritySystem.Rules.Builders;
using Framework.Persistent;

namespace Framework.SecuritySystem;

/// <summary>
/// Сервис с кешированием доступа к контекстным операциям
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TIdent"></typeparam>
public abstract class ContextDomainSecurityServiceBase<TDomainObject, TIdent>(
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
    : DomainSecurityService<TDomainObject>(disabledSecurityProvider, securityRuleExpander)
    where TDomainObject : IIdentityObject<TIdent>
{
    private readonly ISecurityExpressionBuilderFactory securityExpressionBuilderFactory = securityExpressionBuilderFactory
                                                                                          ?? throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));

    protected virtual ISecurityProvider<TDomainObject> Create(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.DomainObjectSecurityRule securityRule)
    {
        if (securityPath == null) throw new ArgumentNullException(nameof(securityPath));
        if (securityRule == null) throw new ArgumentNullException(nameof(securityRule));

        return securityPath.ToProvider(securityRule, this.securityExpressionBuilderFactory);
    }
}

public class ContextDomainSecurityService<TDomainObject, TIdent>(
    ISecurityProvider<TDomainObject> disabledSecurityProvider,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
    SecurityPath<TDomainObject> securityPath)
    : ContextDomainSecurityServiceBase<TDomainObject, TIdent>(
        disabledSecurityProvider,
        securityRuleExpander,
        securityExpressionBuilderFactory)
    where TDomainObject : IIdentityObject<TIdent>
{
    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityRule.ExpandedRolesSecurityRule securityRule)
    {
        return this.Create(securityPath, securityRule);
    }
}
