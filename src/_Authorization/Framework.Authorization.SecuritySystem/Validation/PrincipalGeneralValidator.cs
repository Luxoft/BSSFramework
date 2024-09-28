using FluentValidation;

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PrincipalGeneralValidator : AbstractValidator<Principal>, IPrincipalGeneralValidator
{
    public PrincipalGeneralValidator(
        IValidator<Principal> uniquePermissionValidator,
        IPermissionGeneralValidator permissionGeneralValidator)
    {
        this.Include(uniquePermissionValidator);

        this.RuleForEach(principal => principal.Permissions).SetValidator(permissionGeneralValidator);
    }
}
