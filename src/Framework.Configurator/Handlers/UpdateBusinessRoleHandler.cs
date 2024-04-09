using System.Diagnostics.CodeAnalysis;

using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record UpdateBusinessRoleHandler(
        IRepositoryFactory<BusinessRole> BusinessRoleRepositoryFactory,
        IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IUpdateBusinessRoleHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var roleId = (string?)context.Request.RouteValues["id"] ?? throw new InvalidOperationException();
        var role = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

        await this.Update(new Guid(roleId), role, cancellationToken);
    }

    [SuppressMessage("SonarQube", "S2436", Justification = "It's ok. BusinessRoleOperationLink automatically link to BusinessRole")]
    private async Task Update(Guid id, RequestBodyDto role, CancellationToken cancellationToken)
    {
        var businessRoleBll = this.BusinessRoleRepositoryFactory.Create(SecurityRule.Edit);

        var domainObject = await businessRoleBll.GetQueryable()
                                                .Where(x => x.Id == id)
                                                .SingleAsync(cancellationToken);

        await businessRoleBll.SaveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.BusinessRoleChangedAsync(domainObject, cancellationToken);
        }
    }

    private class RequestBodyDto
    {
    }
}
