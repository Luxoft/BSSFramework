using System;
using System.Linq;
using Automation;
using Automation.ServiceEnvironment;
using Automation.Utils;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Hangfire.Annotations;
using SampleSystem.BLL;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using BusinessRole = SampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers
{
    public class AuthHelper : IntegrationTestContextEvaluator<ISampleSystemBLLContext>
    {
        public AuthHelper(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
        {
        }

        public void SetUserRole(EmployeeIdentityDTO employee, params IPermissionDefinition[] permissions)
        {
            var login = this.EvaluateRead(context => context.Logics.Employee.GetById(employee.Id).Login);
            this.SetUserRole(login, permissions);
        }

        public void SetCurrentUserRole(BusinessRole businessRole)
        {
            this.SetCurrentUserRole(new SampleSystemPermission(businessRole));
        }

        public Framework.Authorization.Generated.DTO.PrincipalIdentityDTO SavePrincipal(string name, bool active, Guid? externalId = null)
        {
            return this.EvaluateWrite(context =>
            {
                var principal = new Framework.Authorization.Domain.Principal { Name = name, Active = active, ExternalId = externalId };
                context.Authorization.Logics.Principal.Save(principal);
                return principal.ToIdentityDTO();
            });
        }

        public string GetCurrentUserLogin()
        {
            return this.EvaluateRead(context => context.Authorization.CurrentPrincipalName);
        }

        public string GetEmployeeLogin(EmployeeIdentityDTO employee)
        {
            return this.Evaluate(DBSessionMode.Read, ctx => ctx.Logics.Employee.GetById(employee.Id, true).Login);
        }

        public void SetCurrentUserRole([NotNull] params IPermissionDefinition[] permissions)
        {
            this.SetUserRole(null, permissions);
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

                    Principal principalDomainObject = principalName == null
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
            this.EvaluateWrite(context =>
            {
                var principalBLL = context.Authorization.Logics.Principal;
                var businessRoleBLL = context.Authorization.Logics.BusinessRole;
                var permissionBLL = context.Authorization.Logics.Permission;
                var currentPrincipal = principalBLL.GetCurrent(true);

                var adminRole = businessRoleBLL.GetOrCreateAdminRole();

                if (!currentPrincipal.Permissions.Select(permission => permission.Role).Contains(adminRole))
                {
                    new Permission(currentPrincipal) { Role = adminRole }.Pipe(permissionBLL.Save);
                }
            });
        }

        protected void FinishRunAsUser()
        {
            this.EvaluateWrite(
                               context =>
                               {
                                   context.Authorization.RunAsManager.FinishRunAsUser();
                               });
        }


        private void FindAndSavePermissionFilter(ISampleSystemBLLContext context, IPermissionDefinition permission, Permission permissionObject)
        {
            foreach (var tuple in permission.GetEntities())
            {
                this.SavePermissionFilter(context, permissionObject, tuple.Item2, tuple.Item1);
            }
        }

        private void SavePermissionFilter(ISampleSystemBLLContext context, Permission permission, Guid entityId, string entityName)
        {
            var entity =
                    context.Authorization.Logics.PermissionFilterEntity.GetUnsecureQueryable()
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

            var item = new PermissionFilterItem(permission)
            {
                Entity = entity
            };
        }

        private void RemovePermissions(string principalName)
        {
            this.EvaluateWrite(
                               context =>
                               {
                                   var permissionBLL = context.Authorization.Logics.Permission;
                                   var principalBLL = context.Authorization.Logics.Principal;

                                   Principal principalDomainObject = principalName == null
                                                                             ? principalBLL.GetCurrent(true)
                                                                             : principalBLL.GetByName(principalName);


                                   if (principalDomainObject == null)
                                   {
                                       return;
                                   }

                                   var permissions =
                                           permissionBLL.GetListBy(p => p.Principal == principalDomainObject);

                                   foreach (Permission permission in permissions)
                                   {
                                       permissionBLL.Remove(permission);
                                   }
                               });
        }
    }
}
