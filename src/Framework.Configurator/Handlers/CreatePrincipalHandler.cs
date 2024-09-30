using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class CreatePrincipalHandler(
    [CurrentUserWithoutRunAs]ISecuritySystem securitySystem,
    IPrincipalManagementService principalManagementService,
    IConfiguratorIntegrationEvents? configuratorIntegrationEvents = null)
    : BaseWriteHandler, ICreatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        securitySystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var name = await this.ParseRequestBodyAsync<string>(context);

        var principal = await principalManagementService.CreatePrincipalAsync(name, cancellationToken);

        if (configuratorIntegrationEvents != null)
            await configuratorIntegrationEvents.PrincipalCreatedAsync(principal, cancellationToken);
    }
}
