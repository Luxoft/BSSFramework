using Automation.ServiceEnvironment.Services;
using Automation.Utils;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using JetBrains.Annotations;
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
        this.RootServiceProvider.GetRequiredService<IntegrationTestUserAuthenticationService>().CustomUserName = principalName;
    }

    public PrincipalIdentityDTO SavePrincipal(string name, bool active, Guid? externalId = null)
    {
        return this.EvaluateWrite(context =>
                                  {
                                      var principal = new Framework.Authorization.Domain.Principal { Name = name, Active = active, ExternalId = externalId };
                                      context.Authorization.Logics.Principal.Save(principal);
                                      return principal.ToIdentityDTO();
                                  });
    }

    public void AddUserRole(string principalName, [NotNull] params IPermissionDefinition[] permissions)
    {
        foreach (IPermissionDefinition permission in permissions)
        {
            IPermissionDefinition currentPermission = permission;

            this.EvaluateWrite(context =>
                               {
                                   var principalBLL = context.Authorization.Logics.Principal;
                                   var businessRoleBLL = context.Authorization.Logics.BusinessRole;
                                   var permissionBLL = context.Authorization.Logics.Permission;

                                   var principalDomainObject = principalName == null
                                                                             ? principalBLL.GetCurrent(true)
                                                                             : principalBLL.GetByNameOrCreate(principalName, true);

                                   var businessRole = businessRoleBLL.GetByName(currentPermission.GetRoleName(), true);

                                   var permissionDomainObject = new Permission(principalDomainObject)
                                                                {
                                                                        Role = businessRole
                                                                };

                                   this.FindAndSavePermissionFilter(context, currentPermission, permissionDomainObject);

                                   permissionBLL.Save(permissionDomainObject);
                               });
        }
    }

    public void SetCurrentUserRole(params IPermissionDefinition[] permissions)
    {
        this.SetUserRole(default(string), permissions);
    }

    public void SetUserRole(string principalName, [NotNull] params IPermissionDefinition[] permissions)
    {
        if (permissions == null)
        {
            throw new ArgumentNullException(nameof(permissions));
        }

        this.RemovePermissions(principalName);

        this.AddUserRole(principalName, permissions);
    }

    public void AddCurrentUserToAdmin()
    {
        this.SetCurrentUserRole(TestBusinessRole.Administrator, TestBusinessRole.SystemIntegration);
    }

    private void FindAndSavePermissionFilter(TBLLContext context, IPermissionDefinition permission, Permission permissionObject)
    {
        foreach (var tuple in permission.GetEntities())
        {
            this.SavePermissionFilter(context, permissionObject, tuple.Item2, tuple.Item1);
        }
    }

    private void SavePermissionFilter(TBLLContext context, Permission permission, Guid entityId, string entityName)
    {
        var entity = context.Authorization.Logics.PermissionFilterEntity.GetUnsecureQueryable()
                            .FirstOrDefault(e => e.EntityId == entityId && e.EntityType.Name == entityName);

        if (entity == null)
        {
            entity = new PermissionFilterEntity
                     {
                             EntityId = entityId,
                             EntityType = context.Authorization.Logics.EntityType.GetByName(entityName)
                     };

            context.Authorization.Logics.PermissionFilterEntity.Save(entity);
        }

        var item = new PermissionFilterItem(permission, entity);
    }

    private void RemovePermissions(string principalName)
    {
        this.EvaluateWrite(context =>
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
