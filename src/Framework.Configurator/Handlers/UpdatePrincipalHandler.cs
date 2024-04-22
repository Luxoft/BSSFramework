using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record UpdatePrincipalHandler(
    IRepositoryFactory<Principal> PrincipalRepoFactory,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null)
    : BaseWriteHandler, IUpdatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principalId = (string?)context.Request.RouteValues["id"]!;
        var name = await this.ParseRequestBodyAsync<string>(context);

        await this.UpdateAsync(new Guid(principalId), name, cancellationToken);
    }

    private async Task UpdateAsync(Guid id, string newName, CancellationToken cancellationToken)
    {
        var domainObject = await this.PrincipalRepoFactory.Create().LoadAsync(id, cancellationToken);
        domainObject.Name = newName;
        await this.PrincipalRepoFactory.Create(SecurityRole.Administrator).SaveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalChangedAsync(domainObject, cancellationToken);
    }
}
