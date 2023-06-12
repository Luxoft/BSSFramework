using Framework.Authorization.BLL.Core.Context;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record DeletePrincipalHandler(
        IRepositoryFactory<Principal> PrincipalRepositoryFactory,
        IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IDeletePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principalId = new Guid((string?)context.Request.RouteValues["id"] ?? throw new InvalidOperationException());
        await this.Delete(principalId, cancellationToken);
    }

    private async Task Delete(Guid id, CancellationToken cancellationToken)
    {
        var principalBll = this.PrincipalRepositoryFactory.Create(BLLSecurityMode.Edit);
        var domainObject = await principalBll.GetQueryable()
                                             .Where(x => x.Id == id)
                                             .SingleAsync(cancellationToken);
        await principalBll.RemoveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.PrincipalRemovedAsync(domainObject, cancellationToken);
        }
    }
}
