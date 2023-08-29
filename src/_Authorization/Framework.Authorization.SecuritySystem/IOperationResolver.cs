using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface IOperationResolver
{
    Operation GetByCode<TSecurityOperationCode>(TSecurityOperationCode securityOperation)
        where TSecurityOperationCode : struct, Enum;
}
