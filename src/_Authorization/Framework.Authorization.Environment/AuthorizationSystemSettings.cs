using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.Authorization.Environment;

public class AuthorizationSystemSettings : IAuthorizationSystemSettings
{
    public Type NotificationPrincipalExtractorType { get; private set; } = typeof(NotificationPrincipalExtractor);

    public List<Action<IServiceCollection>> RegisterActions { get; set; } = new();

    public bool RegisterRunAsManager { get; set; } = true;

    public IAuthorizationSystemSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor
    {
        this.NotificationPrincipalExtractorType = typeof(T);

        return this;
    }

    public IAuthorizationSystemSettings SetUniquePermissionValidator<TValidator>()
        where TValidator : class, IValidator<Principal>
    {
        this.RegisterActions.Add(
            sc => sc.Replace(ServiceDescriptor.KeyedScoped<IValidator<Principal>, TValidator>(PrincipalUniquePermissionValidator.Key)));

        return this;
    }
}
