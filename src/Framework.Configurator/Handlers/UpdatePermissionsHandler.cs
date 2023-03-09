using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class UpdatePermissionsHandler<TBllContext> : BaseWriteHandler, IUpdatePermissionsHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public UpdatePermissionsHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var principalId = (string)context.Request.RouteValues["id"];
            var permissions = await this.ParseRequestBodyAsync<List<RequestBodyDto>>(context);

            this.Update(new Guid(principalId), permissions);
        }

        private void Update(Guid id, IReadOnlyCollection<RequestBodyDto> permissions) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    var cache = new HashSet<PermissionFilterEntity>();
                    var principal = x.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit).GetById(id, true);
                    var mergeResult = principal.Permissions.GetMergeResult(
                        permissions,
                        p => p.Id,
                        p => string.IsNullOrWhiteSpace(p.PermissionId) ? Guid.NewGuid() : new Guid(p.PermissionId));

                    CreatePermissions(x, principal, mergeResult.AddingItems, cache);
                    UpdatePermissions(x, mergeResult.CombineItems, cache);
                    principal.RemoveDetails(mergeResult.RemovingItems);

                    x.Authorization.Logics.Principal.Save(principal);
                });

        private static void CreatePermissions(
            TBllContext context,
            Principal principal,
            IEnumerable<RequestBodyDto> dtoModels,
            ISet<PermissionFilterEntity> cache)
        {
            foreach (var dto in dtoModels)
            {
                var permission = new Permission(principal)
                                 {
                                     Role = context.Authorization.Logics.BusinessRole.GetById(new Guid(dto.RoleId), true),
                                     Comment = dto.Comment,
                                     Status = PermissionStatus.Approved,
                                     Active = true
                                 };
                context.Authorization.Logics.PermissionFactory.Create(BLLSecurityMode.Edit).Save(permission);

                var filterEntities = dto.Contexts.SelectMany(
                    c => c.Entities.Select(
                        e => GetOrCreatePermissionFilterEntity((TypeId: new Guid(c.Id), ObjectId: new Guid(e)), cache, context)));

                foreach (var filterEntity in filterEntities)
                {
                    context.Authorization.Logics.PermissionFilterItem.Save(new PermissionFilterItem(permission, filterEntity));
                }
            }
        }

        private static void UpdatePermissions(
            TBllContext context,
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
                    x => GetOrCreatePermissionFilterEntity((x.TypeId, x.ObjectId), cache, context));

                foreach (var filterEntity in filterEntities)
                {
                    context.Authorization.Logics.PermissionFilterItem.Save(new PermissionFilterItem(item.Item1, filterEntity));
                }

                item.Item1.RemoveDetails(mergeResult.RemovingItems);

                context.Authorization.Logics.PermissionFactory.Create(BLLSecurityMode.Edit).Save(item.Item1);
            }
        }

        private static PermissionFilterEntity GetOrCreatePermissionFilterEntity(
            (Guid TypeId, Guid ObjectId) key,
            ISet<PermissionFilterEntity> cache,
            TBllContext context)
        {
            var result = cache.SingleOrDefault(x => x.EntityType.Id == key.TypeId && x.EntityId == key.ObjectId)
                         ?? context.Authorization.Logics.PermissionFilterEntity.GetUnsecureQueryable()
                                   .SingleOrDefault(x => x.EntityType.Id == key.TypeId && x.EntityId == key.ObjectId);
            if (result != null)
            {
                return result;
            }

            var entityType = context.Authorization.Logics.EntityType.GetById(key.TypeId, true);
            var filterEntity = new PermissionFilterEntity { EntityType = entityType, EntityId = key.ObjectId, Active = true };

            context.Authorization.Logics.PermissionFilterEntity.Save(filterEntity);
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
