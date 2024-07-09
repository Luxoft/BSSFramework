namespace Framework.SecuritySystem;

public class OnlyDisabledDomainSecurityService<TDomainObject>(ISecurityProvider<TDomainObject> disabledSecurityProvider)
    : IDomainSecurityService<TDomainObject>
{
    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityRule securityRule)
    {
        if (securityRule == SecurityRule.Disabled)
        {
            return disabledSecurityProvider;
        }
        else
        {
            throw new InvalidOperationException($"Security mode \"{securityRule}\" not allowed");
        }
    }
}
