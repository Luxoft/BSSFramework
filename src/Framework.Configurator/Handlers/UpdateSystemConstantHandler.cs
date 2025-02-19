using Framework.ApplicationVariable;
using Framework.Configurator.Interfaces;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class UpdateSystemConstantHandler(
    [CurrentUserWithoutRunAs] ISecuritySystem securitySystem,
    IApplicationVariableStorage? variableStorage = null)
    : BaseWriteHandler, IUpdateSystemConstantHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(SecurityRole.Administrator);

        if (variableStorage == null)
        {
            throw new Exception($"{nameof(variableStorage)} not implemented");
        }

        var variableName = (string?)context.Request.RouteValues["name"]!;
        var newValue = await this.ParseRequestBodyAsync<string>(context);

        await variableStorage.UpdateVariableAsync(variableName, newValue, cancellationToken);
    }
}
