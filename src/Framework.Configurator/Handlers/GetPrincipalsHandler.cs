using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public class GetPrincipalsHandler(IRepositoryFactory<Principal> repoFactory, IAuthorizationSystem authorizationSystem)
    : BaseReadHandler, IGetPrincipalsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!authorizationSystem.IsSecurityAdministrator()) return new List<EntityDto>();

        var searchToken = context.Request.Query["searchToken"];

        var query = repoFactory.Create().GetQueryable();
        if (!string.IsNullOrWhiteSpace(searchToken)) query = query.Where(p => p.Name.Contains(searchToken!));

        return await query
                     .Select(x => new EntityDto { Id = x.Id, Name = x.Name })
                     .OrderBy(x => x.Name)
                     .Take(70)
                     .ToListAsync(cancellationToken);
    }
}
