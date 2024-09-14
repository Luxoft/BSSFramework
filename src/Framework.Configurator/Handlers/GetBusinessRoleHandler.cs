using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleHandler(
    ISecuritySystemFactory securitySystemFactory,
    ISecurityRoleSource securityRoleSource,
    ISecurityOperationInfoSource securityOperationInfoSource,
    IPrincipalManagementService configuratorApi)
    : BaseReadHandler, IGetBusinessRoleHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystemFactory.IsSecurityAdministrator()) return new BusinessRoleDetailsDto { Operations = [], Principals = [] };

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
