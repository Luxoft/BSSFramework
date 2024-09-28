using FluentValidation;

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PrincipalDisableUniquePermissionValidator : AbstractValidator<Principal>, IPrincipalUniquePermissionValidator;
