using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleHandler : BaseReadHandler, IGetBusinessRoleHandler

{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetBusinessRoleHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context)
    {
        var roleId = new Guid((string)context.Request.RouteValues["id"] ?? throw new InvalidOperationException());

        var operationDtos = this.authorizationBllContext.Authorization.Logics.OperationFactory.Create(SecurityRule.View)
                                .GetSecureQueryable()
                                .Where(z => z.Links.Any(o => o.BusinessRole.Id == roleId))
                                .Select(o => new OperationDto { Id = o.Id, Name = o.Name, Description = o.Description })
                                .OrderBy(o => o.Name)
                                .Distinct()
                                .ToList();

        var principals = this.authorizationBllContext.Authorization.Logics.PermissionFactory.Create(SecurityRule.View)
                             .GetSecureQueryable()
                             .Where(p => p.Role.Id == roleId)
                             .Select(p => p.Principal.Name)
                             .OrderBy(p => p)
                             .Distinct()
                             .ToList();

        return new BusinessRoleDetailsDto { Operations = operationDtos, Principals = principals };
    }
}
