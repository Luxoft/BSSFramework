using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record DeleteBusinessRoleHandler
        (
        IDefaultRepositoryFactory<BusinessRole> BusinessRoleRepositoryFactory,
        IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IDeleteBusinessRoleHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var roleId = new Guid((string?)context.Request.RouteValues["id"] ?? throw new InvalidOperationException());
        await this.Delete(roleId, cancellationToken);
    }

    private async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var businessRoleBll = this.BusinessRoleRepositoryFactory.Create(BLLSecurityMode.Edit);
        var domainObject = await businessRoleBll.GetQueryable()
                                                .Where(x => x.Id == id)
                                                .SingleAsync(cancellationToken);

        await businessRoleBll.RemoveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.BusinessRoleRemovedAsync(domainObject, cancellationToken);
        }
    }
}
