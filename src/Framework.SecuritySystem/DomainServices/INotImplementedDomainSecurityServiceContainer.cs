namespace Framework.SecuritySystem;

public interface INotImplementedDomainSecurityServiceContainer
{
    IDomainSecurityService<TDomainObject, TSecurityOperationCode> GetNotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode>()
            where TDomainObject : class
            where TSecurityOperationCode : struct, Enum;
}
