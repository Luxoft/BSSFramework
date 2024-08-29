using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.ApplicationCore;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record DeletePrincipalHandler(
    IAuthorizationSystem AuthorizationSystem,
    IRepositoryFactory<Principal> RepoFactory,
    IPrincipalDomainService PrincipalDomainService,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null)
    : BaseWriteHandler, IDeletePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principalId = new Guid((string?)context.Request.RouteValues["id"]!);

        await this.Delete(principalId, cancellationToken);
    }

    private async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        this.AuthorizationSystem.CheckAccess(ApplicationSecurityRule.SecurityAdministrator);

        var principal = await this.RepoFactory.Create().LoadAsync(id, cancellationToken);

        await this.PrincipalDomainService.RemoveAsync(principal, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalRemovedAsync(principal, cancellationToken);
    }
}
