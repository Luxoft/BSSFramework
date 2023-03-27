using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetPrincipalsHandler : BaseReadHandler, IGetPrincipalsHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetPrincipalsHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context)
    {
        var searchToken = context.Request.Query["searchToken"];

        var query = this.authorizationBllContext.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.View)
                        .GetSecureQueryable();
        if (!string.IsNullOrWhiteSpace(searchToken))
        {
            query = query.Where(p => p.Name.Contains(searchToken));
        }

        return query
               .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
               .OrderBy(r => r.Name)
               .Take(70)
               .ToList();
    }
}
