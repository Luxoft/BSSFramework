namespace Framework.SecuritySystem;

public interface IDisabledSecurityProviderContainer<in TPersistentDomainObjectBase>
{
    ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;
}
