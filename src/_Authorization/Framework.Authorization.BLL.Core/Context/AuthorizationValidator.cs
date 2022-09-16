using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.BLL;

public partial class AuthorizationValidator : IAuthorizationValidator
{
    [ActivatorUtilitiesConstructor]
    public AuthorizationValidator(Framework.Authorization.BLL.IAuthorizationBLLContext context, AuthorizationValidatorCompileCache cache) :
            this(context, (ValidatorCompileCache)cache)
    {
    }
}
