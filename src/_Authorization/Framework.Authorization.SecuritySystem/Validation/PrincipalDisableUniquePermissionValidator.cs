using FluentValidation;

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystemImpl.Validation;

public class PrincipalDisableUniquePermissionValidator : AbstractValidator<Principal>, IPrincipalUniquePermissionValidator;
