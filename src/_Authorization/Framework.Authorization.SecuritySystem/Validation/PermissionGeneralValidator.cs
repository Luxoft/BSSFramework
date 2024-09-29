using FluentValidation;

using Framework.Authorization.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionGeneralValidator : AbstractValidator<Permission>, IPermissionGeneralValidator
{
    public PermissionGeneralValidator(
        IValidator<PermissionRestriction> permissionRestrictionValidator,
        [FromKeyedServices(PermissionRequiredContextValidator.Key)] IValidator<Permission> permissionRequiredContextValidator,
        [FromKeyedServices(PermissionDelegateValidator.Key)] IValidator<Permission> permissionDelegateValidator)
    {
        this.RuleForEach(permission => permission.Restrictions).SetValidator(permissionRestrictionValidator);

        this.Include(permissionRequiredContextValidator);
        this.Include(permissionDelegateValidator);
    }
}
