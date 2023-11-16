using Automation.ServiceEnvironment.Services;
using Automation.Utils;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Bss;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public abstract class AuthHelperBase<TBLLContext> : RootServiceProviderContainer<TBLLContext>
    where TBLLContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
{
    protected AuthHelperBase(IServiceProvider rootServiceProvider)
        : base(rootServiceProvider)
    {

    }

    public string GetCurrentUserLogin()
    {
        return this.EvaluateRead(context => context.Authorization.CurrentPrincipalName);
    }


    public void LoginAs(string principalName = null)
    {
        this.RootServiceProvider.GetRequiredService<IIntegrationTestUserAuthenticationService>().SetUserName(principalName);
    }

    public PrincipalIdentityDTO SavePrincipal(string name, bool active, Guid? externalId = null)
    {
        return this.EvaluateWrite(
            context =>
            {
                var principal = new Framework.Authorization.Domain.Principal { Name = name, Active = active, ExternalId = externalId };
                context.Authorization.Logics.Principal.Save(principal);
                return principal.ToIdentityDTO();
            });
    }

    public void AddUserRole(string principalName, params TestPermission[] permissions)
    {
        foreach (var permission in permissions)
        {
            this.EvaluateWrite(
                context =>
                {
                    var principalBLL = context.Authorization.Logics.Principal;
                    var businessRoleBLL = context.Authorization.Logics.BusinessRole;
                    var permissionBLL = context.Authorization.Logics.Permission;

                    var principalDomainObject = principalName == null
                                                    ? principalBLL.GetCurrent(true)
                                                    : principalBLL.GetByNameOrCreate(principalName, true);

                    var businessRole = businessRoleBLL.GetByName(permission.SecurityRoleName, true);

                    var permissionDomainObject = new Permission(principalDomainObject) { Role = businessRole };

                    this.FindAndSavePermissionFilter(context, permission, permissionDomainObject);

                    permissionBLL.Save(permissionDomainObject);
                });
        }
    }

    public void SetCurrentUserRole(params TestPermission[] permissions)
    {
        this.SetUserRole(default, permissions);
    }

    public void SetUserRole(string principalName, params TestPermission[] permissions)
    {
        if (permissions == null)
        {
            throw new ArgumentNullException(nameof(permissions));
        }

        this.RemovePermissions(principalName);

        this.AddUserRole(principalName, permissions);
    }

    public virtual void AddCurrentUserToAdmin()
    {
        this.SetCurrentUserRole(TestPermission.Administrator, TestPermission.SystemIntegration);
    }

    private void FindAndSavePermissionFilter(TBLLContext context, TestPermission permission, Permission permissionObject)
    {
        foreach (var pair in permission.Restrictions)
        {
            foreach (var restriction in pair.Value)
            {
                this.SavePermissionFilter(context, permissionObject, restriction, pair.Key);
            }
        }
    }

    private void SavePermissionFilter(TBLLContext context, Permission permission, Guid entityId, Type securityContextType)
    {
        var securityContextInfo = (ISecurityContextInfo<Guid>)context
                                                              .Authorization.ServiceProvider
                                                              .GetRequiredService<ISecurityContextInfoService>()
                                                              .GetSecurityContextInfo(securityContextType);

        var entityType = context.Authorization.Logics.EntityType.GetById(securityContextInfo.Id)!;

        var entity = context.Authorization.Logics.PermissionFilterEntity.GetUnsecureQueryable()
                            .FirstOrDefault(e => e.EntityId == entityId && e.EntityType == entityType);

        if (entity == null)
        {
            entity = new PermissionFilterEntity { EntityId = entityId, EntityType = entityType };

            context.Authorization.Logics.PermissionFilterEntity.Save(entity);
        }

        var item = new PermissionFilterItem(permission, entity);
    }

    private void RemovePermissions(string principalName)
    {
        this.EvaluateWrite(
            context =>
            {
                var permissionBLL = context.Authorization.Logics.Permission;
                var principalBLL = context.Authorization.Logics.Principal;

                var principalDomainObject = principalName == null
                                                ? principalBLL.GetCurrent(true)
                                                : principalBLL.GetByName(principalName);


                if (principalDomainObject == null)
                {
                    return;
                }

                var permissions =
                    permissionBLL.GetListBy(p => p.Principal == principalDomainObject);

                foreach (var permission in permissions)
                {
                    permissionBLL.Remove(permission);
                }
            });
    }
}
