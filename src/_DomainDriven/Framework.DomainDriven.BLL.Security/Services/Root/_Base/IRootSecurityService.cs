using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface IRootSecurityService<in TPersistentDomainObjectBase>
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
        where TDomainObject : class, TPersistentDomainObjectBase;
}

public interface IRootSecurityService<in TPersistentDomainObjectBase, TSecurityOperationCode> : IRootSecurityService<TPersistentDomainObjectBase>

    where TSecurityOperationCode : struct, Enum
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityOperation<TSecurityOperationCode> securityOperation)
        where TDomainObject : class, TPersistentDomainObjectBase;

    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(TSecurityOperationCode securityOperationCode)
        where TDomainObject : class, TPersistentDomainObjectBase;
}
