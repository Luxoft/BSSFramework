using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public class GetDomainTypesHandler(IRepositoryFactory<DomainType> repoFactory, IOperationAccessor operationAccessor)
    : BaseReadHandler, IGetDomainTypesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!operationAccessor.IsAdmin()) return new List<DomainTypeDto>();

        return await repoFactory.Create()
                                .GetQueryable()
                                .Where(x => x.TargetSystem.IsRevision)
                                .OrderBy(x => x.Name)
                                .Select(
                                    x => new DomainTypeDto
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             Namespace = x.NameSpace,
                                             Operations = x.EventOperations
                                                           .OrderBy(o => o.Name)
                                                           .Select(o => new EntityDto { Id = o.Id, Name = o.Name })
                                                           .ToList()
                                         })
                                .ToListAsync(cancellationToken);
    }
}
