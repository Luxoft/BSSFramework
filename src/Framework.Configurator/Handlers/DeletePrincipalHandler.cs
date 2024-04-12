using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem.Bss;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record DeletePrincipalHandler(
    IRepositoryFactory<Principal> RepoFactory,
    AdministratorRoleInfo AdministratorRoleInfo,
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
        var domainObject = await this.RepoFactory.Create().LoadAsync(id, cancellationToken);
        await this.RepoFactory.Create(this.AdministratorRoleInfo.AdministratorRole).RemoveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalRemovedAsync(domainObject, cancellationToken);
    }
}
