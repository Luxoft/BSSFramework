using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public class AuthorizationSystem : IAuthorizationSystem
{
    public AuthorizationSystem(){}

    public bool IsAdmin() => throw new NotImplementedException();

    public bool HasAccess<TSecurityOperationCode>(NonContextSecurityOperation<TSecurityOperationCode> securityOperation)
        where TSecurityOperationCode : struct, Enum
    {

    }

    public void CheckAccess<TSecurityOperationCode>(NonContextSecurityOperation<TSecurityOperationCode> operation)
        where TSecurityOperationCode : struct, Enum
    {

    }

    public string ResolveSecurityTypeName(Type type)
    {
    }
}
