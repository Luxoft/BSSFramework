using FluentValidation;

using Framework.Authorization.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionGeneralValidator : AbstractValidator<Permission>
{
    public const string Key = "General";

    public PermissionGeneralValidator(
        [FromKeyedServices(PermissionRestrictionValidator.Key)] IValidator<Permission> permissionRestrictionValidator,
        [FromKeyedServices(PermissionDelegateValidator.Key)] IValidator<Permission> permissionDelegateValidator)
    {
        this.Include(permissionRestrictionValidator);
        this.Include(permissionDelegateValidator);
    }
}
