using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetPrincipalsHandler([CurrentUserWithoutRunAs]ISecuritySystem securitySystem, IPrincipalManagementService configuratorApi)
    : BaseReadHandler, IGetPrincipalsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsSecurityAdministrator()) return new List<EntityDto>();

        var nameFilter = (string)context.Request.Query["searchToken"]!;

        var principals = await configuratorApi.GetPrincipalsAsync(nameFilter, 70, cancellationToken);

        return principals
               .Select(x => new PrincipalHeaderDto { Id = x.Id, Name = x.Name, IsVirtual = x.IsVirtual })
               .OrderBy(x => x.Name)
               .ToList();
    }
}
