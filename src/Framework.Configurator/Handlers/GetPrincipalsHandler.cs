using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetPrincipalsHandler(IAuthorizationBLLContext authorizationBllContext) : BaseReadHandler, IGetPrincipalsHandler
{
    protected override Task<object> GetData(HttpContext context)
    {
        var searchToken = context.Request.Query["searchToken"];

        var query = authorizationBllContext.Authorization.Logics.PrincipalFactory.Create(SecurityRule.View)
                                           .GetSecureQueryable();
        if (!string.IsNullOrWhiteSpace(searchToken)) query = query.Where(p => p.Name.Contains(searchToken));

        return Task.FromResult<object>(
            query
                .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
                .OrderBy(r => r.Name)
                .Take(70)
                .ToList());
    }
}
