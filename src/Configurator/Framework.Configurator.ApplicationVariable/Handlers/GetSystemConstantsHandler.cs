using Anch.SecuritySystem;
using Anch.SecuritySystem.Attributes;
using Anch.SecuritySystem.Configurator.Handlers;

using Framework.Application.ApplicationVariable;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetSystemConstantsHandler(
    [WithoutRunAs] ISecuritySystem securitySystem,
    IApplicationVariableStorage variableStorage)
    : BaseReadHandler, IGetSystemConstantsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken ct)
    {
        if (await securitySystem.HasAccessAsync(SecurityRole.Administrator, ct))
        {
            var variables = await variableStorage.GetVariablesAsync(ct);

            return variables.Select(x => new SystemConstantDto { Name = x.Key.Name, Description = x.Key.Description, Value = x.Value })
                            .OrderBy(x => x.Name);
        }
        else
        {
            return new List<SystemConstantDto>();
        }
    }
}

