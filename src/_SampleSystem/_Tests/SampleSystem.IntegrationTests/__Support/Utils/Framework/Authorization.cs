using System;
using System.Linq;

using Automation.Utils;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests.__Support.Utils.Framework
{
    public abstract class Authorization
    {
        public abstract void EvaluateWrite(Action<ISampleSystemBLLContext> action);

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

                    permissionBLL.Save(permissionDomainObject);

                    this.FindAndSavePermissionFilter(context, currentPermission,
                                                     permissionDomainObject);
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

            context.Authorization.Logics.PermissionFilterItem.Save(item);
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
                                           permissionBLL.GetObjectsBy(p => p.Principal == principalDomainObject);

                                   foreach (Permission permission in permissions)
                                   {
                                       permissionBLL.Remove(permission);
                                   }
                               });
        }
    }
}
