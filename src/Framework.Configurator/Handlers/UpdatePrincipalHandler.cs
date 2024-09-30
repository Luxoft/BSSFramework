using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class UpdatePrincipalHandler(
    [CurrentUserWithoutRunAs]ISecuritySystem securitySystem,
    IPrincipalManagementService principalManagementService,
    IConfiguratorIntegrationEvents? configuratorIntegrationEvents = null)
    : BaseWriteHandler, IUpdatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principalId = new Guid((string?)context.Request.RouteValues["id"]!);

        var principalName = await this.ParseRequestBodyAsync<string>(context);

        var principal = await principalManagementService.UpdatePrincipalNameAsync(principalId, principalName, cancellationToken);

        if (configuratorIntegrationEvents != null)
            await configuratorIntegrationEvents.PrincipalChangedAsync(principal, cancellationToken);
    }
}
