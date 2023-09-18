namespace Framework.SecuritySystem;

public interface IDomainSecurityService<TDomainObject>
{
    ISecurityProvider<TDomainObject> GetSecurityProvider(BLLSecurityMode securityMode);

    ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation securityOperation);
}
