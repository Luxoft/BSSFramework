using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem.Validation;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemSettings
{
    bool RegisterRunAsManager { get; set; }

    IAuthorizationSystemSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor;

    IAuthorizationSystemSettings SetUniquePermissionValidator<TValidator>()
        where TValidator : class, IPrincipalUniquePermissionValidator;
}
