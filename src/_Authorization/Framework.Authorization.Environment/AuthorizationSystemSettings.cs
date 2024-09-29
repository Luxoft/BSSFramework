using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.Validation;
using Framework.SecuritySystem.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment;

public class AuthorizationSystemSettings : IAuthorizationSystemSettings
{
    private Type notificationPrincipalExtractorType = typeof(NotificationPrincipalExtractor);

    private Type principalUniquePermissionValidatorType = typeof(PrincipalUniquePermissionValidator);

    public bool RegisterRunAsManager { get; set; } = true;

    public IAuthorizationSystemSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor
    {
        this.notificationPrincipalExtractorType = typeof(T);

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
        services.AddScoped(typeof(INotificationPrincipalExtractor), this.notificationPrincipalExtractorType);

        services.AddScoped(typeof(IPrincipalUniquePermissionValidator), this.principalUniquePermissionValidatorType);

        if (this.RegisterRunAsManager)
        {
            services.AddScoped<IRunAsManager, AuthorizationRunAsManager>();
        }
    }
}
