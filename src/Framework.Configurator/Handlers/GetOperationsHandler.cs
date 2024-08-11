using Framework.Authorization.Environment.Security;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetOperationsHandler(IOperationAccessor operationAccessor, ISecurityRoleSource roleSource, ISecurityOperationInfoSource operationInfoSource)
    : BaseReadHandler, IGetOperationsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!operationAccessor.IsSecurityAdministrator()) return new List<string>();

        var operations = roleSource.SecurityRoles
                                   .SelectMany(x => x.Information.Operations)
                                   .Select(
                                       o => new OperationDto
                                            {
                                                Name = o.Name, Description = operationInfoSource.GetSecurityOperationInfo(o).Description
                                            })
                                   .OrderBy(x => x.Name)
                                   .DistinctBy(x => x.Name)
                                   .ToList();

        return operations;
    }
}
