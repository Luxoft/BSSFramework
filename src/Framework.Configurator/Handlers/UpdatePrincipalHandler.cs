using Framework.Authorization.BLL.Core.Context;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record UpdatePrincipalHandler(
        IAuthorizationRepositoryFactory<Principal> PrincipalRepositoryFactory,
        IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, IUpdatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principalId = (string?)context.Request.RouteValues["id"] ?? throw new InvalidOperationException();
        var name = await this.ParseRequestBodyAsync<string>(context);

        await this.Update(new Guid(principalId), name, cancellationToken);
    }

    private async Task Update(Guid id, string newName, CancellationToken cancellationToken)
    {
        var principalBll = this.PrincipalRepositoryFactory.Create(BLLSecurityMode.Edit);
        var domainObject = await principalBll.GetQueryable()
                                             .Where(x => x.Id == id)
                                             .SingleAsync(cancellationToken);
        domainObject.Name = newName;
        await principalBll.SaveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.PrincipalChangedAsync(domainObject, cancellationToken);
        }
    }
}
