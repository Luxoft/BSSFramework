using FluentValidation;

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystemImpl.Validation;

public interface IPrincipalGeneralValidator : IValidator<Principal>;
