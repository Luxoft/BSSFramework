namespace Framework.SecuritySystem.Services;

public interface ISecurityProviderInjector<TDomainObject, in TSecurityRule> : ISecurityProviderFactory<TDomainObject, TSecurityRule>
    where TSecurityRule : SecurityRule;


public class SecurityProviderInjector<TDomainObject, TSecurityRule> : ISecurityProviderInjector<TDomainObject, TSecurityRule>
    where TSecurityRule : SecurityRule
{
    internal Func<TSecurityRule, SecurityPath<TDomainObject>, ISecurityProvider<TDomainObject>> DefaultCreateFunc { get; set; } = default!;


    public virtual ISecurityProvider<TDomainObject> Create(TSecurityRule securityRule, SecurityPath<TDomainObject> securityPath) =>
        this.DefaultCreateFunc.Invoke(securityRule, securityPath);
}
