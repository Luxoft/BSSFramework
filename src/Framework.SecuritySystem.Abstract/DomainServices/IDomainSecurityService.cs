namespace Framework.SecuritySystem;

public interface IDomainSecurityService<TDomainObject>
{
    ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityRule securityRule);
}
