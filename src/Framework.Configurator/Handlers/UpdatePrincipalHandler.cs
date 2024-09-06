using Framework.Configurator.Interfaces;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record UpdatePrincipalHandler(
    ISecuritySystem SecuritySystem,
    IConfiguratorApi ConfiguratorApi,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null)
    : BaseWriteHandler, IUpdatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.SecuritySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principalId = new Guid((string?)context.Request.RouteValues["id"]!);

        var principalName = await this.ParseRequestBodyAsync<string>(context);

        await this.ConfiguratorApi.UpdatePrincipalNameAsync(principalId, principalName, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalChangedAsync(principalId, cancellationToken);
    }
}
