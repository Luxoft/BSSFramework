using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.Notification;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemSettings
{
    bool RegisterRunAsManager { get; set; }

    IAuthorizationSystemSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor;

    IAuthorizationSystemSettings SetUniquePermissionValidator<TValidator>()
        where TValidator : class, IValidator<Principal>;
}
