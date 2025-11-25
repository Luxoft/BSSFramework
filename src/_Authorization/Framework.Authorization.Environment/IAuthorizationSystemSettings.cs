using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystemImpl.Validation;

namespace Framework.Authorization.Environment;

public interface IAuthorizationSystemSettings
{
    bool RegisterRunAsManager { get; set; }

    IAuthorizationSystemSettings SetNotificationPermissionExtractor<T>()
        where T : INotificationPermissionExtractor;

    IAuthorizationSystemSettings SetUniquePermissionValidator<TValidator>()
        where TValidator : class, IPrincipalUniquePermissionValidator;
}
