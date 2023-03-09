using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class GetPrincipalHandler<TBllContext> : BaseReadHandler, IGetPrincipalHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetPrincipalHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context)
        {
            var principalId = new Guid((string)context.Request.RouteValues["id"]);

            return this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x =>
                {
                    var permissions = GetPermissions(x, principalId);
                    var contexts = GetContexts(x, permissions);
                    return new PrincipalDetailsDto { Permissions = ToDto(permissions, contexts) };
                });
        }

        private static List<PermissionDetails> GetPermissions(TBllContext context, Guid principalId) =>
            context.Authorization.Logics.PermissionFactory.Create(BLLSecurityMode.View)
                   .GetSecureQueryable()
                   .Where(p => p.Principal.Id == principalId)
                   .Select(
                       x => new PermissionDetails
                            {
                                Id = x.Id,
                                Role = x.Role.Name,
                                Comment = x.Comment,
                                Contexts = x.FilterItems.Select(f => new KeyValuePair<Guid, Guid>(f.EntityType.Id, f.Entity.EntityId))
                                            .ToList()
                            })
                   .ToList();

        private static Dictionary<Guid, (string Context, Dictionary<Guid, string> Entities)> GetContexts(
            TBllContext context,
            IEnumerable<PermissionDetails> permissions)
        {
            var result = new Dictionary<Guid, (string Context, Dictionary<Guid, string> Entities)>();
            foreach (var group in permissions.SelectMany(x => x.Contexts).GroupBy(x => x.Key, x => x.Value))
            {
                var entityType = context.Authorization.Logics.EntityType.GetById(group.Key);
                var entities = context.Authorization.ExternalSource.GetTyped(entityType)
                                      .GetSecurityEntitiesByIdents(group.Distinct().ToList())
                                      .ToDictionary(e => e.Id, e => e.Name);

                result.Add(entityType.Id, (Context: entityType.Name, Entities: entities));
            }

            return result;
        }

        private static List<PermissionDto> ToDto(
            IEnumerable<PermissionDetails> permissions,
            IReadOnlyDictionary<Guid, (string Context, Dictionary<Guid, string> Entities)> contexts) =>
            permissions
                .Select(
                    x => new PermissionDto
                         {
                             Id = x.Id,
                             Role = x.Role,
                             Comment = x.Comment,
                             Contexts = x.Contexts
                                         .GroupBy(c => c.Key, c => c.Value)
                                         .Select(
                                             g => new ContextDto
                                                  {
                                                      Id = g.Key,
                                                      Name = contexts[g.Key].Context,
                                                      Entities = g.Select(e => new EntityDto { Id = e, Name = contexts[g.Key].Entities[e] })
                                                                  .OrderBy(e => e.Name)
                                                                  .ToList()
                                                  })
                                         .OrderBy(c => c.Name)
                                         .ToList()
                         })
                .OrderBy(x => x.Role)
                .ToList();

        private class PermissionDetails
        {
            public Guid Id
            {
                get;
                set;
            }

            public string Role
            {
                get;
                set;
            }

            public string Comment
            {
                get;
                set;
            }

            public List<KeyValuePair<Guid, Guid>> Contexts
            {
                get;
                set;
            }
        }
    }
}
