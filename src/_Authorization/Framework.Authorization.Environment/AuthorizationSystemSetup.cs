using Anch.Core;
using Anch.DependencyInjection;
using Anch.RelativePath.DependencyInjection;

using Framework.Application;
using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem;
using Anch.SecuritySystem.DependencyInjection;
using Anch.SecuritySystem.ExternalSystem.ApplicationSecurity;
using Anch.SecuritySystem.GeneralPermission.DependencyInjection;
using Anch.SecuritySystem.GeneralPermission.Validation;
using Anch.SecuritySystem.Notification.DependencyInjection;
using Anch.SecuritySystem.UserSource;

namespace Framework.Authorization.Environment;

public class AuthorizationSystemSetup : IAuthorizationSystemSetup, IServiceInitializer<ISecuritySystemSetup>
{
    private Type? uniquePermissionComparerType;

    public bool RegisterRunAsManager { get; set; } = true;

    public IAuthorizationSystemSetup SetUniquePermissionComparer<TComparer>()
        where TComparer : class, IPermissionEqualityComparer<Permission, PermissionRestriction>
    {
        this.uniquePermissionComparerType = typeof(TComparer);

        return this;
    }

    public void Initialize(ISecuritySystemSetup settings)
    {
        var securityAdministratorRule = ApplicationSecurityRule.SecurityAdministrator;
        var principalViewSecurityRule = securityAdministratorRule.Or(DomainSecurityRule.CurrentUser);
        var delegatedFromSecurityRule = new DomainSecurityRule.CurrentUserSecurityRule { RelativePathKey = nameof(Permission.DelegatedFrom) };

        settings.AddExtensions(services => services.AddScoped<IDalGenericInterceptor<Permission>, MasterDetailDalGenericInterceptor<Permission, Principal>>()
                                       .AddScoped<IDalGenericInterceptor<PermissionRestriction>, MasterDetailDalGenericInterceptor<PermissionRestriction, Permission>>())

                .AddUserSource<Principal>(usb =>
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
                         .SetPermissionComment(v => v.Comment)
                         .SetPermissionDelegation(p => p.DelegatedFrom))

                .AddDomainSecurity<Principal>(b => b.SetView(principalViewSecurityRule)
                                                    .SetEdit(securityAdministratorRule))

                .AddDomainSecurity<Permission>(b => b.SetView(principalViewSecurityRule.Or(delegatedFromSecurityRule))
                                                     .SetEdit(securityAdministratorRule.Or(delegatedFromSecurityRule)))

                .AddDomainSecurity<BusinessRole>(b => b.SetView(securityAdministratorRule.Or(AuthorizationSecurityRule.AvailableBusinessRole))
                                                       .SetEdit(securityAdministratorRule))

                .AddDomainSecurity<SecurityContextType>(b => b.SetView(SecurityRule.Disabled))

                .AddNotification()

                .AddExtensions(services =>
                {
                    if (this.uniquePermissionComparerType != null)
                    {
                        services.AddScoped(typeof(IPermissionEqualityComparer<Permission, PermissionRestriction>), this.uniquePermissionComparerType);
                    }

                    services.AddRelativeDomainPath((Permission permission) => permission.Principal)
                            .AddRelativeDomainPath((Permission permission) => permission.DelegatedFrom!.Principal, nameof(Permission.DelegatedFrom))

                            .AddRelativeDomainPath((BusinessRole businessRole) => businessRole)
                            .AddScoped(typeof(AvailableBusinessRoleSecurityProvider<>));
                });
    }
}
