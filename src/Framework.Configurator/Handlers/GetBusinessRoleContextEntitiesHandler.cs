using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextEntitiesHandler : BaseReadHandler, IGetBusinessRoleContextEntitiesHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetBusinessRoleContextEntitiesHandler(IAuthorizationBLLContext authorizationBllContext) =>
        this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context)
    {
        var securityContextTypeId = new Guid((string)context.Request.RouteValues["id"] ?? throw new InvalidOperationException());
        var searchToken = context.Request.Query["searchToken"];

        var securityContextType = this.authorizationBllContext.Authorization.Logics.SecurityContextTypeFactory
            .Create(SecurityRule.View)
            .GetById(securityContextTypeId, true);

        var entities = this.authorizationBllContext.Authorization.ExternalSource.GetTyped(securityContextType)
            .GetSecurityEntities();

        if (!string.IsNullOrWhiteSpace(searchToken))
        {
            entities = entities.Where(p => p.Name.Contains(searchToken, StringComparison.OrdinalIgnoreCase));
        }

        return entities
            .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
            .OrderByDescending(r => r.Name.Equals(searchToken, StringComparison.OrdinalIgnoreCase))
            .ThenBy(r => r.Name)
            .Take(70)
            .ToList();
    }
}
