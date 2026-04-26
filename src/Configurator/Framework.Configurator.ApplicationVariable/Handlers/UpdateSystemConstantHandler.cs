using Framework.Application.ApplicationVariable;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Attributes;
using Anch.SecuritySystem.Configurator.Handlers;

namespace Framework.Configurator.Handlers;

public class UpdateSystemConstantHandler(
    [WithoutRunAs] ISecuritySystem securitySystem,
    IApplicationVariableStorage variableStorage)
    : BaseWriteHandler, IUpdateSystemConstantHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        await securitySystem.CheckAccessAsync(SecurityRole.Administrator, cancellationToken);

        var variableName = (string?)context.Request.RouteValues["name"]!;
        var newValue = await this.ParseRequestBodyAsync<string>(context);

        await variableStorage.UpdateVariableAsync(variableName, newValue, cancellationToken);
    }
}
