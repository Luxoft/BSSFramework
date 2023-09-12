using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface IRootSecurityService<in TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
        where TDomainObject : TPersistentDomainObjectBase;

    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityOperation securityOperation)
        where TDomainObject : TPersistentDomainObjectBase;
}
