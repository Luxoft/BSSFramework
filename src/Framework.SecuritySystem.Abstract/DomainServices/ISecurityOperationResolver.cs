namespace Framework.SecuritySystem;

public interface ISecurityOperationResolver<in TPersistentDomainObjectBase>
{
    SecurityOperation GetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
        where TDomainObject : TPersistentDomainObjectBase;
}
