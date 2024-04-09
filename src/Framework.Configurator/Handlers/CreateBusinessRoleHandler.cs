using System.Diagnostics.CodeAnalysis;

using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record CreateBusinessRoleHandler(
        IRepositoryFactory<BusinessRole> BusinessRoleRepositoryFactory,
        IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, ICreateBusinessRoleHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var newRole = await this.ParseRequestBodyAsync<RequestBodyDto>(context);
        await this.Create(newRole, cancellationToken);
    }

    [SuppressMessage("SonarQube", "S2436", Justification = "It's ok. BusinessRoleOperationLink automatically link to BusinessRole")]
    private async Task Create(RequestBodyDto newRole, CancellationToken cancellationToken)
    {
        var domainObject = new BusinessRole { Name = newRole.Name };

        await this.BusinessRoleRepositoryFactory.Create(SecurityRule.Edit).SaveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.BusinessRoleCreatedAsync(domainObject, cancellationToken);
        }
    }

    private class RequestBodyDto
    {
        public string Name
        {
            get;
            set;
        } = default!;
    }
}
