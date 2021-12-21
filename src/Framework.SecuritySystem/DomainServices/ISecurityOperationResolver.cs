using System;

namespace Framework.SecuritySystem
{
    public interface ISecurityOperationResolver<in TPersistentDomainObjectBase, TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum
    {
        SecurityOperation<TSecurityOperationCode> GetSecurityOperation(TSecurityOperationCode securityOperationCode);

        SecurityOperation<TSecurityOperationCode> GetSecurityOperation<TDomainObject>(BLLSecurityMode securityMode)
            where TDomainObject : TPersistentDomainObjectBase;
    }
}
