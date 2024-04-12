using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record CreatePrincipalHandler(
    IRepositoryFactory<Principal> RepoFactory,
    IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null)
    : BaseWriteHandler, ICreatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var name = await this.ParseRequestBodyAsync<string>(context);
        var principal = new Principal { Name = name };

        await this.RepoFactory.Create(SpecialRoleSecurityRule.Administrator).SaveAsync(principal, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
            await this.ConfiguratorIntegrationEvents.PrincipalCreatedAsync(principal, cancellationToken);
    }
}
