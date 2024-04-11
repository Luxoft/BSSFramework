using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetOperationsHandler(IOperationAccessor operationAccessor, ISecurityRoleSource roleSource)
    : BaseReadHandler, IGetOperationsHandler
{
    protected override Task<object> GetData(HttpContext context)
    {
        if (!operationAccessor.IsAdmin()) return Task.FromResult<object>(new List<string>());

        var operations = roleSource.SecurityRoles
                                   .SelectMany(x => x.Operations)
                                   .Select(o => new OperationDto { Name = o.Name, Description = o.Description })
                                   .OrderBy(x => x.Name)
                                   .DistinctBy(x => x.Name)
                                   .ToList();

        return Task.FromResult<object>(operations);
    }
}
