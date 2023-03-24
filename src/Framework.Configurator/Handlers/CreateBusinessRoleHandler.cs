using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.BLL.Core.Context;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record CreateBusinessRoleHandler(
        IAuthorizationRepositoryFactory<Operation> OperationRepositoryFactory,
        IAuthorizationRepositoryFactory<BusinessRole> BusinessRoleRepositoryFactory,
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
        var operationIds = await this.OperationRepositoryFactory.Create()
                                     .GetQueryable()
                                     .Where(x => newRole.OperationIds.Contains(x.Id))
                                     .ToListAsync(cancellationToken);

        foreach (var operation in operationIds)
        {
            var _ = new BusinessRoleOperationLink(domainObject) { Operation = operation };
        }

        await this.BusinessRoleRepositoryFactory.Create(BLLSecurityMode.Edit).SaveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.BusinessRoleCreatedAsync(domainObject, cancellationToken);
        }
    }

    private class RequestBodyDto
    {
        public List<Guid> OperationIds
        {
            get;
            set;
        } = default!;

        public string Name
        {
            get;
            set;
        } = default!;
    }
}
