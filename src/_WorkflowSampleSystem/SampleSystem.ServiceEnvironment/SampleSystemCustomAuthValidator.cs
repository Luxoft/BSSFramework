using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Validation;

namespace SampleSystem.ServiceEnvironment
{
    /// <summary>
    /// Кастомный валидатор авторизации
    /// </summary>
    public class SampleSystemCustomAuthValidator : AuthorizationValidator
    {
        public SampleSystemCustomAuthValidator(IAuthorizationBLLContext context, ValidatorCompileCache cache)
                : base(context, cache)
        {
        }

#pragma warning disable S1185 // Overriding members should do more than simply call the same member in the base class
        protected override ValidationResult GetUniquePermissionsValidationResult(Principal principal)
        {
            // Тут можно написать "return ValidationResult.Success", тогда валидация по пермиссиям будет отключена
            return base.GetUniquePermissionsValidationResult(principal);
        }

        protected override ValidationResult GetUniqueValidationResult(Permission permission)
        {
            // Тут можно написать "return ValidationResult.Success", тогда валидация по соседним пермиссиям будет отключена
            return base.GetUniqueValidationResult(permission);
        }
#pragma warning restore S1185 // Overriding members should do more than simply call the same member in the base class
    }
}
