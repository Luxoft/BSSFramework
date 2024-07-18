﻿using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextsHandler(
    IRepositoryFactory<SecurityContextType> contextTypeRepoFactory,
    IOperationAccessor operationAccessor)
    : BaseReadHandler, IGetBusinessRoleContextsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!operationAccessor.IsSecurityAdministrator()) return new List<EntityDto>();

        return await contextTypeRepoFactory
                     .Create()
                     .GetQueryable()
                     .Select(x => new EntityDto { Id = x.Id, Name = x.Name })
                     .OrderBy(x => x.Name)
                     .ToListAsync(cancellationToken);
    }
}
