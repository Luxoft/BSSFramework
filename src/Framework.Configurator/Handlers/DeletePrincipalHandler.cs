using Framework.Configurator.Interfaces;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record DeletePrincipalHandler(
    ISecuritySystem SecuritySystem,
    IPrincipalManagementService PrincipalManagementService,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null)
    : BaseWriteHandler, IDeletePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.SecuritySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principalId = new Guid((string?)context.Request.RouteValues["id"]!);

        await this.PrincipalManagementService.RemovePrincipalAsync(principalId, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalRemovedAsync(principalId, cancellationToken);
    }
}
