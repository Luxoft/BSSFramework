using FluentValidation;

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystemImpl.Validation;

public class PrincipalGeneralValidator : AbstractValidator<Principal>, IPrincipalGeneralValidator
{
    public PrincipalGeneralValidator(
        IPrincipalUniquePermissionValidator uniquePermissionValidator,
        IPermissionGeneralValidator permissionGeneralValidator)
    {
        this.Include(uniquePermissionValidator);

        this.RuleForEach(principal => principal.Permissions).SetValidator(permissionGeneralValidator);
    }
}
