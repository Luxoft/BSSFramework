using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record CreatePrincipalHandler(
    IAuthorizationSystem AuthorizationSystem,
    IPrincipalManageService PrincipalManageService,
    IRepositoryFactory<Principal> RepoFactory,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null)
    : BaseWriteHandler, ICreatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.AuthorizationSystem.CheckAccess(SecurityRole.Administrator);

        var name = await this.ParseRequestBodyAsync<string>(context);

        var principal = await this.PrincipalManageService.GetOrCreateAsync(name, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalCreatedAsync(principal, cancellationToken);
    }
}
