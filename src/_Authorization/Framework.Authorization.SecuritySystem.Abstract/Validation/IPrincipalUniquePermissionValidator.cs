using FluentValidation;

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.Validation;

public interface IPrincipalUniquePermissionValidator : IValidator<Principal>;
