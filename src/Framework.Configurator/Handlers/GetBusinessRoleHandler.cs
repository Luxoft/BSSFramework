using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleHandler(
    ISecuritySystem securitySystem,
    ISecurityRoleSource securityRoleSource,
    ISecurityOperationInfoSource securityOperationInfoSource,
    IConfiguratorApi configuratorApi)
    : BaseReadHandler, IGetBusinessRoleHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsSecurityAdministrator()) return new BusinessRoleDetailsDto { Operations = [], Principals = [] };

        var securityRoleId = new Guid((string)context.Request.RouteValues["id"]!);

        var securityRole = securityRoleSource.GetSecurityRole(securityRoleId);

        var operations =
            securityRole
                .Information
                .Operations
                .Select(
                    o => new OperationDto
                         {
                             Name = o.Name, Description = securityOperationInfoSource.GetSecurityOperationInfo(o).Description
                         })
                .OrderBy(x => x.Name)
                .ToList();

        var principals = await configuratorApi.GetLinkedPrincipalsAsync([securityRole], cancellationToken);

        return new BusinessRoleDetailsDto { Operations = operations, Principals = principals.ToList() };
    }
}
