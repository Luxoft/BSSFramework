using Framework.Configurator.Interfaces;
using Framework.DomainDriven.ApplicationSecurity;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class DeletePrincipalHandler(
    [CurrentUserWithoutRunAs]ISecuritySystem securitySystem,
    IPrincipalManagementService principalManagementService,
    IConfiguratorIntegrationEvents? configuratorIntegrationEvents = null)
    : BaseWriteHandler, IDeletePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principalId = new Guid((string?)context.Request.RouteValues["id"]!);

        var principal = await principalManagementService.RemovePrincipalAsync(principalId, false, cancellationToken);

        if (configuratorIntegrationEvents != null)
            await configuratorIntegrationEvents.PrincipalRemovedAsync(principal, cancellationToken);
    }
}
