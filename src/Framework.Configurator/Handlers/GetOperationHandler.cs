using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetOperationHandler(
    ISecuritySystem securitySystem,
    ISecurityRoleSource roleSource,
    IPrincipalManagementService configuratorApi)
    : BaseReadHandler, IGetOperationHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsSecurityAdministrator()) return new OperationDetailsDto { BusinessRoles = [], Principals = [] };

        var operationName = (string)context.Request.RouteValues["name"]!;

        var securityRoles = roleSource.SecurityRoles
                                      .Where(x => x.Information.Operations.Any(o => o.Name == operationName))
                                      .ToList();

        var principals = await configuratorApi.GetLinkedPrincipalsAsync(securityRoles, cancellationToken);

        return new OperationDetailsDto
               {
                   BusinessRoles = securityRoles.Select(x => x.Name).Order().ToList(), Principals = principals.ToList()
               };
    }
}
