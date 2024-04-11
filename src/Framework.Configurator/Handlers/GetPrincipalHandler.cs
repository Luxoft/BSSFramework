﻿using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetPrincipalHandler(IAuthorizationBLLContext authorizationBllContext) : BaseReadHandler, IGetPrincipalHandler
{
    protected override Task<object> GetData(HttpContext context)
    {
        var principalId = new Guid((string)context.Request.RouteValues["id"] ?? throw new InvalidOperationException());

        var permissions = this.GetPermissions(principalId);
        var contexts = this.GetContexts(permissions);
        return Task.FromResult<object>(new PrincipalDetailsDto { Permissions = ToDto(permissions, contexts) });
    }

    private List<PermissionDetails> GetPermissions(Guid principalId) =>
        authorizationBllContext.Authorization.Logics.PermissionFactory.Create(SecurityRule.View)
                               .GetSecureQueryable()
                               .Where(p => p.Principal.Id == principalId)
                               .Select(
                                   x => new PermissionDetails
                                        {
                                            Id = x.Id,
                                            Role = x.Role.Name,
                                            Comment = x.Comment,
                                            StartDate = x.Period.StartDate,
                                            EndDate = x.Period.EndDate,
                                            Contexts = x.Restrictions
                                                        .Select(
                                                            f => new KeyValuePair<Guid, Guid>(
                                                                f.SecurityContextType.Id,
                                                                f.SecurityContextId))
                                                        .ToList()
                                        })
                               .ToList();

    private Dictionary<Guid, (string Context, Dictionary<Guid, string> Entities)> GetContexts(
        IEnumerable<PermissionDetails> permissions)
    {
        var result = new Dictionary<Guid, (string Context, Dictionary<Guid, string> Entities)>();
        foreach (var group in permissions.SelectMany(x => x.Contexts).GroupBy(x => x.Key, x => x.Value))
        {
            var securityContextType = authorizationBllContext.Authorization.Logics.SecurityContextType.GetById(group.Key, true);
            var entities = authorizationBllContext.Authorization.ExternalSource.GetTyped(securityContextType)
                                                  .GetSecurityEntitiesByIdents(group.Distinct().ToList())
                                                  .ToDictionary(e => e.Id, e => e.Name);

            result.Add(securityContextType.Id, (Context: securityContextType.Name, Entities: entities));
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
                         StartDate = x.StartDate,
                         EndDate = x.EndDate,
                         Contexts = x.Contexts
                                     .GroupBy(c => c.Key, c => c.Value)
                                     .Select(
                                         g => new ContextDto
                                              {
                                                  Id = g.Key,
                                                  Name = contexts[g.Key].Context,
                                                  Entities = g
                                                             .Select(
                                                                 e => new EntityDto
                                                                      {
                                                                          Id = e,
                                                                          Name = contexts[g.Key]
                                                                              .Entities[e]
                                                                      })
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
        public Guid Id { get; set; }

        public string Role { get; set; }

        public string Comment { get; set; }

        public List<KeyValuePair<Guid, Guid>> Contexts { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime StartDate { get; set; }
    }
}
