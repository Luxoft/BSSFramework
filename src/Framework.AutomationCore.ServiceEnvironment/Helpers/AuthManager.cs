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
    IPermissionValidator permissionValidator)
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

    public void AddUserRole(string principalName, params TestPermission[] permissions)
    {
        var actualPrincipalName = principalName ?? this.GetCurrentUserLogin();

        var principalDomainObject = principalRepository.GetQueryable().SingleOrDefault(p => p.Name == actualPrincipalName)
                                    ?? new Principal { Name = actualPrincipalName };

        foreach (var testPermission in permissions)
        {
            var securityRole = securityRoleSource.GetFullRole(testPermission.SecurityRole);

            var businessRole = businessRoleRepository.Load(securityRole.Id);

            var permissionDomainObject = new Permission(principalDomainObject) { Role = businessRole, Period = testPermission.Period };

            foreach (var pair in testPermission.Restrictions)
            {
                var securityContextInfo = (ISecurityContextInfo<Guid>)securityContextInfoService
                    .GetSecurityContextInfo(pair.Key);

                var domainSecurityContextType = securityContextTypeRepository.Load(securityContextInfo.Id);

                foreach (var securityContextId in pair.Value)
                {
                    new PermissionRestriction(permissionDomainObject) { SecurityContextType = domainSecurityContextType, SecurityContextId = securityContextId };
                }
            }

            permissionValidator.Validate(permissionDomainObject);
        }

        principalRepository.SaveAsync(principalDomainObject).GetAwaiter().GetResult();
    }

    public void RemovePermissions(string principalName)
    {
        var actualPrincipalName = principalName ?? this.GetCurrentUserLogin();

        var principalDomainObject = principalRepository.GetQueryable().SingleOrDefault(p => p.Name == actualPrincipalName);

        if (principalDomainObject != null)
        {
            principalDomainObject.ClearDetails<Principal, Permission>();

            principalRepository.SaveAsync(principalDomainObject).GetAwaiter().GetResult();
        }
    }
}
