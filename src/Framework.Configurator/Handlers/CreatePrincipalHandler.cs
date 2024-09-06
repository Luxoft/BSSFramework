using Framework.Configurator.Interfaces;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record CreatePrincipalHandler(
    ISecuritySystem SecuritySystem,
    IPrincipalManagementService PrincipalManagementService,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null)
    : BaseWriteHandler, ICreatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.SecuritySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var name = await this.ParseRequestBodyAsync<string>(context);

        var principal = await this.PrincipalManagementService.CreatePrincipalAsync(name, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalCreatedAsync(principal, cancellationToken);
    }
}
