using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAuthOperationResolver
{
    Operation GetAuthOperation(SecurityOperation securityOperation);
}
