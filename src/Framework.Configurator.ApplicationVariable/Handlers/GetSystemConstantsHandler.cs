using Framework.ApplicationVariable;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using SecuritySystem;

using Microsoft.AspNetCore.Http;

using SecuritySystem.Attributes;
using SecuritySystem.Configurator.Handlers;

namespace Framework.Configurator.Handlers;

public class GetSystemConstantsHandler(
    [CurrentUserWithoutRunAs] ISecuritySystem securitySystem,
    IApplicationVariableStorage variableStorage)
    : BaseReadHandler, IGetSystemConstantsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsAdministrator()) return new List<SystemConstantDto>();

        var variables = await variableStorage.GetVariablesAsync(cancellationToken);

        return variables.Select(x => new SystemConstantDto { Name = x.Key.Name, Description = x.Key.Description, Value = x.Value })
                        .OrderBy(x => x.Name);
    }
}
