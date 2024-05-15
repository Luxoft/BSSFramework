using FluentValidation;

using Framework.Authorization.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionGeneralValidator : AbstractValidator<Permission>
{
    public const string Key = "General";

    public PermissionGeneralValidator(
        IValidator<PermissionRestriction> permissionRestrictionValidator,
        [FromKeyedServices(PermissionDelegateValidator.Key)] IValidator<Permission> permissionDelegateValidator)
    {
        this.RuleForEach(permission => permission.Restrictions).SetValidator(permissionRestrictionValidator);

        this.Include(permissionDelegateValidator);
    }
}
