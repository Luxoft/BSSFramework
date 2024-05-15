using Automation.Utils;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public class AuthManager(
    IUserAuthenticationService userAuthenticationService,
    ISecurityContextInfoService securityContextInfoService,
    [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<Principal> principalRepository,
    [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<BusinessRole> businessRoleRepository,
    [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<SecurityContextType> securityContextTypeRepository,
    ISecurityRoleSource securityRoleSource,
    IPrincipalGeneralValidator principalGeneralValidator)
{
    public string GetCurrentUserLogin()
    {
        return userAuthenticationService.GetUserName();
    }

    public Guid SavePrincipal(string name, bool active, Guid? externalId = null)
    {
        var principal = new Framework.Authorization.Domain.Principal { Name = name, Active = active, ExternalId = externalId };

        principalRepository.SaveAsync(principal).GetAwaiter().GetResult();

        return principal.Id;
    }

    public void AddUserRole(string principalName, params TestPermission[] testPermissions)
    {
        var actualPrincipalName = principalName ?? this.GetCurrentUserLogin();

        var principal = principalRepository.GetQueryable().SingleOrDefault(p => p.Name == actualPrincipalName)
                                    ?? new Principal { Name = actualPrincipalName };

        foreach (var testPermission in testPermissions)
        {
            var securityRole = securityRoleSource.GetFullRole(testPermission.SecurityRole);

            var businessRole = businessRoleRepository.Load(securityRole.Id);

            var permission = new Permission(principal) { Role = businessRole, Period = testPermission.Period };

            foreach (var restrictionInfo in testPermission.Restrictions)
            {
                var securityContextInfo = (ISecurityContextInfo<Guid>)securityContextInfoService
                    .GetSecurityContextInfo(restrictionInfo.Key);

                var domainSecurityContextType = securityContextTypeRepository.Load(securityContextInfo.Id);

                foreach (var securityContextId in restrictionInfo.Value)
                {
                    new PermissionRestriction(permission) { SecurityContextType = domainSecurityContextType, SecurityContextId = securityContextId };
                }
            }
        }

        principalGeneralValidator.Validate(principal);

        principalRepository.SaveAsync(principal).GetAwaiter().GetResult();
    }

    public void RemovePermissions(string principalName)
    {
        var actualPrincipalName = principalName ?? this.GetCurrentUserLogin();

        var principal = principalRepository.GetQueryable().SingleOrDefault(p => p.Name == actualPrincipalName);

        if (principal != null)
        {
            principal.ClearDetails<Principal, Permission>();

            principalRepository.SaveAsync(principal).GetAwaiter().GetResult();
        }
    }
}
