using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Authorization.SecuritySystem.Initialize;
using Framework.Authorization.SecuritySystem.Validation;
using Framework.DependencyInjection;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.ApplicationCore;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorizationSystem(this IServiceCollection services, Action<IAuthorizationSystemSettings>? setup = null)
    {
        services.RegisterGeneralAuthorizationSystem()
                .RegisterAuthorizationSecurity()
                .RegistryGenericDatabaseVisitors()
                .CustomizeSecuritySystem();

        var settings = new AuthorizationSystemSettings();

        setup?.Invoke(settings);

        settings.RegisterActions.ForEach(action => action(services));

        services.AddScoped<INotificationBasePermissionFilterSource, NotificationBasePermissionFilterSource>();
        services.AddScoped(typeof(INotificationPrincipalExtractor), settings.NotificationPrincipalExtractorType);

        return services;
    }

    private static IServiceCollection RegisterGeneralAuthorizationSystem(this IServiceCollection services)
    {
        return services.AddScoped<IRunAsManager, RunAsManger>()

                       .AddScoped<IAvailablePermissionSource, AvailablePermissionSource>()
                       .AddScoped<ICurrentPrincipalSource, CurrentPrincipalSource>()

                       .AddScoped<IAuthorizationSystemFactory, AuthorizationSystemFactory>()

                       .AddScoped<IAvailableSecurityRoleSource, AvailableSecurityRoleSource>()
                       .AddScoped<IAvailableSecurityOperationSource, AvailableSecurityOperationSource>()

                       .AddSingleton<InitializerSettings>()
                       .AddScoped<IAuthorizationSecurityContextInitializer, AuthorizationSecurityContextInitializer>()
                       .AddScoped<IAuthorizationBusinessRoleInitializer, AuthorizationBusinessRoleInitializer>()

                       .AddScoped<IAuthorizationExternalSource, AuthorizationExternalSource>()
                       .AddScoped(typeof(LocalStorage<>))

                       .AddScoped<IPrincipalDomainService, PrincipalDomainService>()

                       .AddScoped<IPrincipalGeneralValidator, PrincipalGeneralValidator>()
                       .AddKeyedScoped<IValidator<Principal>, PrincipalUniquePermissionValidator>(PrincipalUniquePermissionValidator.Key)
                       .AddKeyedScoped<IValidator<Permission>, PermissionGeneralValidator>(PermissionGeneralValidator.Key)
                       .AddKeyedScoped<IValidator<Permission>, PermissionDelegateValidator>(PermissionDelegateValidator.Key)
                       .AddKeyedScoped<IValidator<Permission>, PermissionRequiredContextValidator>(PermissionRequiredContextValidator.Key)
                       .AddScoped<IValidator<PermissionRestriction>, PermissionRestrictionValidator>();
    }

    private static IServiceCollection RegisterAuthorizationSecurity(this IServiceCollection services)
    {
        var securityAdministratorRule = ApplicationSecurityRule.SecurityAdministrator;

        var principalViewSecurityRule = securityAdministratorRule.Or(AuthorizationSecurityRule.CurrentPrincipal);

        return services

               .AddSingleton<SecurityAdministratorRuleFactory>()

               .AddRelativeDomainPath((Permission permission) => permission.Principal)
               .AddScoped(typeof(CurrentPrincipalSecurityProvider<>))

               .AddRelativeDomainPath((BusinessRole businessRole) => businessRole)
               .AddScoped(typeof(AvailableBusinessRoleSecurityProvider<>))

               .AddRelativeDomainPath((Permission permission) => permission.DelegatedFrom, nameof(Permission.DelegatedFrom))
               .AddScoped(typeof(DelegatedFromSecurityProvider<>))

               .RegisterDomainSecurityServices(
                   rb => rb
                         .Add<Principal>(
                             b => b.SetView(principalViewSecurityRule)
                                   .SetEdit(securityAdministratorRule))

                         .Add<Permission>(
                             b => b.SetView(principalViewSecurityRule.Or(AuthorizationSecurityRule.DelegatedFrom))
                                   .SetEdit(securityAdministratorRule.Or(AuthorizationSecurityRule.DelegatedFrom)))

                         .Add<BusinessRole>(
                             b => b.SetView(securityAdministratorRule.Or(AuthorizationSecurityRule.AvailableBusinessRole))
                                   .SetEdit(securityAdministratorRule))

                         .Add<SecurityContextType>(
                             b => b.SetView(SecurityRule.Disabled)));
    }

    private static IServiceCollection RegistryGenericDatabaseVisitors(this IServiceCollection services)
    {
        return services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();
    }

    private static IServiceCollection CustomizeSecuritySystem(this IServiceCollection services)
    {
        return services.ReplaceScoped<ICurrentUser, AuthorizationCurrentUser>()
                       .AddScoped<AuthorizationPermissionSystem>();
    }
}
