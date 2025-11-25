using CommonFramework.DependencyInjection;

using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystemImpl;
using Framework.Authorization.SecuritySystemImpl.Initialize;
using Framework.Authorization.SecuritySystemImpl.Validation;
using Framework.DomainDriven._Visitors;

using SecuritySystem;
using SecuritySystem.Credential;
using SecuritySystem.DependencyInjection;
using SecuritySystem.ExternalSystem;
using SecuritySystem.ExternalSystem.ApplicationSecurity;
using SecuritySystem.ExternalSystem.Management;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.Environment;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAuthorizationSystem(Action<IAuthorizationSystemSettings>? setup = null)
        {
            services.RegisterGeneralAuthorizationSystem()
                    .RegisterAuthorizationSecurity()
                    .RegistryGenericDatabaseVisitors()
                    .UpdateSecuritySystem();

            var settings = new AuthorizationSystemSettings();

            setup?.Invoke(settings);

            settings.Initialize(services);

            return services;
        }

        private IServiceCollection RegisterGeneralAuthorizationSystem()
        {
            return services.AddScoped<IPrincipalResolver, PrincipalResolver>()
                           .AddScoped<IUserCredentialNameByIdResolver, AuthorizationUserCredentialNameByIdResolver>()

                           .AddScoped<INotificationGeneralPermissionFilterFactory, NotificationGeneralPermissionFilterFactory>()
                           .AddScoped<INotificationPrincipalExtractor, NotificationPrincipalExtractor>()

                           .AddScoped<IAvailablePermissionSource, AvailablePermissionSource>()
                           .AddScoped<ICurrentPrincipalSource, CurrentPrincipalSource>()

                           .AddSingleton<InitializerSettings>()
                           .AddScoped<IAuthorizationSecurityContextInitializer, AuthorizationSecurityContextInitializer>()
                           .AddScoped<IAuthorizationBusinessRoleInitializer, AuthorizationBusinessRoleInitializer>()

                           .AddScoped<IPrincipalDomainService, PrincipalDomainService>()

                           .AddScoped<IPrincipalGeneralValidator, PrincipalGeneralValidator>()
                           .AddScoped<IPermissionGeneralValidator, PermissionGeneralValidator>()
                           .AddKeyedScoped<IValidator<Permission>, PermissionDelegateValidator>(PermissionDelegateValidator.Key)
                           .AddKeyedScoped<IValidator<Permission>, PermissionRequiredContextValidator>(PermissionRequiredContextValidator.Key)
                           .AddScoped<IValidator<PermissionRestriction>, PermissionRestrictionValidator>();
        }

        private IServiceCollection RegisterAuthorizationSecurity()
        {
            var securityAdministratorRule = ApplicationSecurityRule.SecurityAdministrator;

            var principalViewSecurityRule = securityAdministratorRule.Or(AuthorizationSecurityRule.CurrentPrincipal);

            return services

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

        private IServiceCollection RegistryGenericDatabaseVisitors()
        {
            return services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<PersistentDomainObjectBase, Guid>>();
        }

        private IServiceCollection UpdateSecuritySystem()
        {
            return services.AddScoped<IPermissionSystemFactory, AuthorizationPermissionSystemFactory>()
                           .AddScoped<AuthorizationPrincipalManagementService>()
                           .AddScopedFrom<IPrincipalSourceService, AuthorizationPrincipalManagementService>()
                           .ReplaceScopedFrom<IPrincipalManagementService, AuthorizationPrincipalManagementService>();
        }
    }
}
