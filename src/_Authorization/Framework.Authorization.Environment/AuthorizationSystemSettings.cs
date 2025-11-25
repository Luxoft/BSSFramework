using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystemImpl;
using Framework.Authorization.SecuritySystemImpl.Validation;
using SecuritySystem.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment;

public class AuthorizationSystemSettings : IAuthorizationSystemSettings
{
    private Type notificationPermissionExtractorType = typeof(NotificationPermissionExtractor);

    private Type principalUniquePermissionValidatorType = typeof(PrincipalUniquePermissionValidator);

    public bool RegisterRunAsManager { get; set; } = true;

    public IAuthorizationSystemSettings SetNotificationPermissionExtractor<T>()
        where T : INotificationPermissionExtractor
    {
        this.notificationPermissionExtractorType = typeof(T);

        return this;
    }

    public IAuthorizationSystemSettings SetUniquePermissionValidator<TValidator>()
        where TValidator : class, IPrincipalUniquePermissionValidator
    {
        this.principalUniquePermissionValidatorType = typeof(TValidator);

        return this;
    }
    public void Initialize(IServiceCollection services)
    {
        services.AddScoped(typeof(INotificationPermissionExtractor), this.notificationPermissionExtractorType);
        services.AddScoped(typeof(IPrincipalUniquePermissionValidator), this.principalUniquePermissionValidatorType);

        if (this.RegisterRunAsManager)
        {
            services.AddScoped<IRunAsManager, AuthorizationRunAsManager>();
        }
    }
}
