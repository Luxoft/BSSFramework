using SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public interface IRootSecurityService<in TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class
{
    ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(SecurityRule securityRule)
        where TDomainObject : TPersistentDomainObjectBase;
}
