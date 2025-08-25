using Framework.ApplicationVariable;
using Framework.Configurator.Interfaces;
using SecuritySystem;

using Microsoft.AspNetCore.Http;

using SecuritySystem.Attributes;
using SecuritySystem.Configurator.Handlers;

namespace Framework.Configurator.Handlers;

public class UpdateSystemConstantHandler(
    [CurrentUserWithoutRunAs] ISecuritySystem securitySystem,
    IApplicationVariableStorage variableStorage)
    : BaseWriteHandler, IUpdateSystemConstantHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(SecurityRole.Administrator);

        var variableName = (string?)context.Request.RouteValues["name"]!;
        var newValue = await this.ParseRequestBodyAsync<string>(context);

        await variableStorage.UpdateVariableAsync(variableName, newValue, cancellationToken);
    }
}
