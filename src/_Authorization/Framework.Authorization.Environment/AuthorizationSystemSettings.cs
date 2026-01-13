using CommonFramework;
using CommonFramework.RelativePath.DependencyInjection;

using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Authorization.Environment.Validation;
using Framework.Authorization.Notification;
using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.DependencyInjection;
using SecuritySystem.ExternalSystem.ApplicationSecurity;
using SecuritySystem.GeneralPermission.DependencyInjection;
using SecuritySystem.GeneralPermission.Validation;
using SecuritySystem.GeneralPermission.Validation.Permission;
using SecuritySystem.UserSource;

namespace Framework.Authorization.Environment;

public class AuthorizationSystemSettings : IAuthorizationSystemSettings
{
    private Type notificationPermissionExtractorType = typeof(NotificationPermissionExtractor);

    private Type? uniquePermissionComparerType;

    public bool RegisterRunAsManager { get; set; } = true;

    public IAuthorizationSystemSettings SetNotificationPermissionExtractor<T>()
        where T : INotificationPermissionExtractor
    {
        this.notificationPermissionExtractorType = typeof(T);

        return this;
    }

    public IAuthorizationSystemSettings SetUniquePermissionComparer<TComparer>()
        where TComparer : class, IPermissionEqualityComparer<Permission, PermissionRestriction>
    {
        this.uniquePermissionComparerType = typeof(TComparer);

        return this;
    }

    public void Initialize(ISecuritySystemSettings settings)
    {
        var securityAdministratorRule = ApplicationSecurityRule.SecurityAdministrator;
        var principalViewSecurityRule = securityAdministratorRule.Or(DomainSecurityRule.CurrentUser);
        var delegatedFromSecurityRule = new DomainSecurityRule.CurrentUserSecurityRule(nameof(Permission.DelegatedFrom));

        settings.AddUserSource<Principal>(usb =>
                {
                    usb.SetMissedService<CreateVirtualMissedUserService<Principal>>();

                    if (this.RegisterRunAsManager)
                    {
                        usb.SetRunAs(p => p.RunAs);
                    }
                })
                .AddRunAsValidator<ExistsOtherwiseUsersRunAsValidator<Principal>>()

                .AddGeneralPermission(
                    p => p.Principal,
                    p => p.Role,
                    (PermissionRestriction pr) => pr.Permission,
                    pr => pr.SecurityContextType,
                    pr => pr.SecurityContextId,
                    b => b
                         .SetSecurityRoleDescription(sr => sr.Description)
                         .SetPermissionPeriod(
                             new PropertyAccessors<Permission, DateTime?>(
                                 v => v.Period.StartDate,
                                 v => v.Period.StartDate,
                                 (permission, startDate) => permission.Period = new Period(startDate ?? DateTime.MinValue, permission.Period.EndDate)),
                             new PropertyAccessors<Permission, DateTime?>(
                                 v => v.Period.EndDate,
                                 v => v.Period.EndDate,
                                 (permission, endDate) => permission.Period = new Period(permission.Period.StartDate, endDate)))
                         .SetPermissionComment(v => v.Comment))

                .AddDomainSecurity<Principal>(b => b.SetView(principalViewSecurityRule)
                                                    .SetEdit(securityAdministratorRule))

                .AddDomainSecurity<Permission>(b => b.SetView(principalViewSecurityRule.Or(delegatedFromSecurityRule))
                                                     .SetEdit(securityAdministratorRule.Or(delegatedFromSecurityRule)))

                .AddDomainSecurity<BusinessRole>(b => b.SetView(securityAdministratorRule.Or(AuthorizationSecurityRule.AvailableBusinessRole))
                                                       .SetEdit(securityAdministratorRule))

                .AddDomainSecurity<SecurityContextType>(b => b.SetView(SecurityRule.Disabled))

                .AddExtensions(services =>
                {
                    if (this.uniquePermissionComparerType != null)
                    {
                        services.AddScoped(typeof(IPermissionEqualityComparer<Permission, PermissionRestriction>), this.uniquePermissionComparerType);
                    }

                    services.AddRelativeDomainPath((Permission permission) => permission.Principal)
                            .AddRelativeDomainPath((Permission permission) => permission.DelegatedFrom!.Principal, nameof(Permission.DelegatedFrom))

                            .AddRelativeDomainPath((BusinessRole businessRole) => businessRole)
                            .AddScoped(typeof(AvailableBusinessRoleSecurityProvider<>))
                            .AddScoped(typeof(INotificationPermissionExtractor), this.notificationPermissionExtractorType)

                            .AddScoped<INotificationGeneralPermissionFilterFactory, NotificationGeneralPermissionFilterFactory>()
                            .AddScoped<INotificationPrincipalExtractor, NotificationPrincipalExtractor>()

                            .AddScoped<IPermissionValidator<Permission, PermissionRestriction>, PermissionDelegateValidator>();
                });
    }
}
