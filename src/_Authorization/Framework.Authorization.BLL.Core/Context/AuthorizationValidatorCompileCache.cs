using Framework.Validation;

namespace Framework.Authorization.BLL;

public class AuthorizationValidatorCompileCache : ValidatorCompileCache
{
    public AuthorizationValidatorCompileCache(AuthorizationValidationMap authorizationValidationMap) :
            base(authorizationValidationMap)
    {
    }
}
