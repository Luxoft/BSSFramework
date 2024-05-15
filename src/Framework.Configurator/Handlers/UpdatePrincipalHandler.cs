using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configurator.Handlers;

public record UpdatePrincipalHandler(
    [FromKeyedServices(nameof(SecurityRole.Administrator))] IRepository<Principal> PrincipalRepository,
    IPrincipalValidator PrincipalValidator,
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
        var domainObject = await this.PrincipalRepository.LoadAsync(id, cancellationToken);
        domainObject.Name = newName;
        await this.PrincipalRepository.SaveAsync(domainObject, cancellationToken);
        this.PrincipalValidator.GetValidateResult(domainObject);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalChangedAsync(domainObject, cancellationToken);
    }
}
