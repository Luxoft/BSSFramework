using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleHandler(IAuthorizationBLLContext authorizationBllContext) : BaseReadHandler, IGetBusinessRoleHandler

{
    protected override Task<object> GetData(HttpContext context)
    {
        var roleId = new Guid((string)context.Request.RouteValues["id"] ?? throw new InvalidOperationException());

        var principals = authorizationBllContext.Authorization.Logics.PermissionFactory.Create(SecurityRule.View)
                                                .GetSecureQueryable()
                                                .Where(p => p.Role.Id == roleId)
                                                .Select(p => p.Principal.Name)
                                                .OrderBy(p => p)
                                                .Distinct()
                                                .ToList();

        return Task.FromResult<object>(new BusinessRoleDetailsDto { Operations = new List<OperationDto>(), Principals = principals });
    }
}
