using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class UpdatePermissionsHandler : BaseWriteHandler, IUpdatePermissionsHandler
    {
        private readonly IAuthorizationBLLContext authorizationBllContext;

        public UpdatePermissionsHandler(IAuthorizationBLLContext authorizationBllContext) => this.authorizationBllContext = authorizationBllContext;

        public async Task Execute(HttpContext context)
        {
            var principalId = (string)context.Request.RouteValues["id"];
            var permissions = await this.ParseRequestBodyAsync<List<RequestBodyDto>>(context);

            this.Update(new Guid(principalId), permissions);
        }

        private void Update(Guid id, IReadOnlyCollection<RequestBodyDto> permissions)
        {
            
                    var cache = new HashSet<PermissionFilterEntity>();
                    var principal =  this.authorizationBllContext.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit).GetById(id, true);
                    
                    var mergeResult = principal.Permissions.GetMergeResult(
                        permissions,
                        p => p.Id,
                        p => string.IsNullOrWhiteSpace(p.PermissionId) ? Guid.NewGuid() : new Guid(p.PermissionId));

                    this.CreatePermissions(principal, mergeResult.AddingItems, cache);
                    this.UpdatePermissions( mergeResult.CombineItems, cache);
                    principal.RemoveDetails(mergeResult.RemovingItems);

                    this.authorizationBllContext.Authorization.Logics.Principal.Save(principal);

        }

        private void CreatePermissions(
                Principal principal,
            IEnumerable<RequestBodyDto> dtoModels,
            ISet<PermissionFilterEntity> cache)
        {
            foreach (var dto in dtoModels)
            {
                var permission = new Permission(principal)
                                 {
                                     Role = this.authorizationBllContext.Authorization.Logics.BusinessRole.GetById(new Guid(dto.RoleId), true),
                                     Comment = dto.Comment,
                                     Status = PermissionStatus.Approved,
                                     Active = true
                                 };
                
                this.authorizationBllContext.Authorization.Logics.PermissionFactory.Create(BLLSecurityMode.Edit).Save(permission);

                var filterEntities = dto.Contexts.SelectMany(
                    c => c.Entities.Select(
                        e => this.GetOrCreatePermissionFilterEntity((TypeId: new Guid(c.Id), ObjectId: new Guid(e)), cache)));

                foreach (var filterEntity in filterEntities)
                {
                    this.authorizationBllContext.Authorization.Logics.PermissionFilterItem.Save(new PermissionFilterItem(permission, filterEntity));
                }
            }
        }

        private void UpdatePermissions(
                IList<TupleStruct<Permission, RequestBodyDto>> updatedItems,
            ISet<PermissionFilterEntity> cache)
        {
            foreach (var item in updatedItems)
            {
                item.Item1.Comment = item.Item2.Comment;

                var mergeResult = item.Item1.FilterItems.GetMergeResult(
                    item.Item2.Contexts.SelectMany(x => x.Entities.Select(e => new { TypeId = new Guid(x.Id), ObjectId = new Guid(e) })),
                    x => new { TypeId = x.EntityType.Id, ObjectId = x.Entity.EntityId },
                    x => new { x.TypeId, x.ObjectId });

                var filterEntities = mergeResult.AddingItems.Select(
                    x => this.GetOrCreatePermissionFilterEntity((x.TypeId, x.ObjectId), cache));

                foreach (var filterEntity in filterEntities)
                {
                    this.authorizationBllContext.Authorization.Logics.PermissionFilterItem.Save(new PermissionFilterItem(item.Item1, filterEntity));
                }

                item.Item1.RemoveDetails(mergeResult.RemovingItems);

                this.authorizationBllContext.Authorization.Logics.PermissionFactory.Create(BLLSecurityMode.Edit).Save(item.Item1);
            }
        }

        private  PermissionFilterEntity GetOrCreatePermissionFilterEntity(
            (Guid TypeId, Guid ObjectId) key,
            ISet<PermissionFilterEntity> cache)
        {
            var result = cache.SingleOrDefault(x => x.EntityType.Id == key.TypeId && x.EntityId == key.ObjectId)
                         ?? this.authorizationBllContext.Authorization.Logics.PermissionFilterEntity.GetUnsecureQueryable()
                                .SingleOrDefault(x => x.EntityType.Id == key.TypeId && x.EntityId == key.ObjectId);
            if (result != null)
            {
                return result;
            }

            var entityType = this.authorizationBllContext.Authorization.Logics.EntityType.GetById(key.TypeId, true);
            var filterEntity = new PermissionFilterEntity { EntityType = entityType, EntityId = key.ObjectId, Active = true };

            this.authorizationBllContext.Authorization.Logics.PermissionFilterEntity.Save(filterEntity);
            cache.Add(filterEntity);

            return filterEntity;
        }

        private class RequestBodyDto
        {
            public string PermissionId
            {
                get;
                set;
            }

            public string RoleId
            {
                get;
                set;
            }

            public string Comment
            {
                get;
                set;
            }

            public List<ContextDto> Contexts
            {
                get;
                set;
            }

            public class ContextDto
            {
                public string Id
                {
                    get;
                    set;
                }

                public List<string> Entities
                {
                    get;
                    set;
                }
            }
        }
    }
}
