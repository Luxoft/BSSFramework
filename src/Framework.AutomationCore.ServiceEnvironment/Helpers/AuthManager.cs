using Automation.Utils;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
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
    IPrincipalDomainService principalDomainService)
{
    public string GetCurrentUserLogin()
    {
        return userAuthenticationService.GetUserName();
    }

    public async Task<Guid> SavePrincipalAsync(string name, CancellationToken cancellationToken = default)
    {
        var principal = await principalDomainService.GetOrCreateAsync(name, cancellationToken);

        return principal.Id;
    }

    public async Task AddUserRoleAsync(string principalName, TestPermission[] testPermissions, CancellationToken cancellationToken = default)
    {
        var principal = await principalDomainService.GetOrCreateAsync(principalName ?? this.GetCurrentUserLogin(), cancellationToken);

        foreach (var testPermission in testPermissions)
        {
            var securityRole = securityRoleSource.GetFullRole(testPermission.SecurityRole);

            if (securityRole.IsVirtual)
            {
                throw new Exception($"Assigned {nameof(SecurityRole)} {securityRole} can't be virtual");
            }

            var businessRole = await businessRoleRepository.LoadAsync(securityRole.Id, cancellationToken);

            var permission = new Permission(principal) { Role = businessRole, Period = testPermission.Period };

            foreach (var restrictionInfo in testPermission.Restrictions)
            {
                var securityContextInfo = (ISecurityContextInfo<Guid>)securityContextInfoService
                    .GetSecurityContextInfo(restrictionInfo.Key);

                var domainSecurityContextType = await securityContextTypeRepository.LoadAsync(securityContextInfo.Id, cancellationToken);

                foreach (var securityContextId in restrictionInfo.Value)
                {
                    new PermissionRestriction(permission) { SecurityContextType = domainSecurityContextType, SecurityContextId = securityContextId };
                }
            }
        }

        await principalDomainService.SaveAsync(principal, cancellationToken);
    }

    public async Task RemovePermissionsAsync(string principalName, CancellationToken cancellationToken = default)
    {
        var actualPrincipalName = principalName ?? this.GetCurrentUserLogin();

        var principal = principalRepository.GetQueryable().SingleOrDefault(p => p.Name == actualPrincipalName);

        if (principal != null)
        {
            await principalDomainService.RemoveAsync(principal, true, cancellationToken);
        }
    }
}
