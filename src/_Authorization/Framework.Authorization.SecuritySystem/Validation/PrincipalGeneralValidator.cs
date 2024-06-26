﻿using FluentValidation;

using Framework.Authorization.Domain;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PrincipalGeneralValidator : AbstractValidator<Principal>, IPrincipalGeneralValidator
{
    //public const string Key = "General";

    public PrincipalGeneralValidator(
        [FromKeyedServices(PrincipalUniquePermissionValidator.Key)] IValidator<Principal> uniquePermissionValidator,
        [FromKeyedServices(PermissionGeneralValidator.Key)] IValidator<Permission> permissionGeneralValidator)
    {
        this.Include(uniquePermissionValidator);

        this.RuleForEach(principal => principal.Permissions).SetValidator(permissionGeneralValidator);
    }
}
