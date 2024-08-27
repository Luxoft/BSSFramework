using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public class GetSystemConstantsHandler(IRepositoryFactory<SystemConstant> repoFactory, IAuthorizationSystem authorizationSystem)
    : BaseReadHandler, IGetSystemConstantsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!authorizationSystem.IsAdministrator()) return new List<SystemConstantDto>();

        return await repoFactory
                     .Create()
                     .GetQueryable()
                     .Select(x => new SystemConstantDto { Id = x.Id, Name = x.Code, Description = x.Description, Value = x.Value })
                     .OrderBy(x => x.Name)
                     .ToListAsync(cancellationToken);
    }
}
