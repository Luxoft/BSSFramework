using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface IRootSecurityService<in TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
        where TDomainObject : TPersistentDomainObjectBase;
}

public interface
    IRootSecurityService<in TPersistentDomainObjectBase, TSecurityOperationCode> : IRootSecurityService<TPersistentDomainObjectBase>

    where TSecurityOperationCode : struct, Enum
    where TPersistentDomainObjectBase : class
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityOperation<TSecurityOperationCode> securityOperation)
        where TDomainObject : TPersistentDomainObjectBase;

    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(TSecurityOperationCode securityOperationCode)
        where TDomainObject : TPersistentDomainObjectBase;
}
