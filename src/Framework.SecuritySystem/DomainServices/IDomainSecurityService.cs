namespace Framework.SecuritySystem;

public interface IDomainSecurityService<TDomainObject>
{
    ISecurityProvider<TDomainObject> GetSecurityProvider(BLLSecurityMode securityMode);
}

public interface IDomainSecurityService<TDomainObject, TSecurityOperationCode> : IDomainSecurityService<TDomainObject>

        where TSecurityOperationCode : struct, Enum
{
    ISecurityProvider<TDomainObject> GetSecurityProvider(TSecurityOperationCode securityOperationCode);

    ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation<TSecurityOperationCode> securityOperation);
}
