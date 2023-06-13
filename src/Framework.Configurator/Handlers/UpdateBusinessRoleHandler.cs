using System.Diagnostics.CodeAnalysis;

using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public record UpdateBusinessRoleHandler(
        IRepositoryFactory<BusinessRole> BusinessRoleRepositoryFactory,
        IRepositoryFactory<Operation> OperationRepositoryFactory,
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
        var businessRoleBll = this.BusinessRoleRepositoryFactory.Create(BLLSecurityMode.Edit);

        var domainObject = await businessRoleBll.GetQueryable()
                                                .Where(x => x.Id == id)
                                                .SingleAsync(cancellationToken);

        var mergeResult =
                domainObject.BusinessRoleOperationLinks.GetMergeResult(
                                                                       role.OperationIds,
                                                                       s => s.Operation.Id,
                                                                       s => s);

        var operations = await this.OperationRepositoryFactory.Create()
                                   .GetQueryable()
                                   .Where(x => mergeResult.AddingItems.Contains(x.Id))
                                   .ToListAsync(cancellationToken);

        foreach (var operation in operations)
        {
            var _ = new BusinessRoleOperationLink(domainObject) { Operation = operation };
        }

        domainObject.RemoveDetails(mergeResult.RemovingItems);
        await businessRoleBll.SaveAsync(domainObject, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.BusinessRoleChangedAsync(domainObject, cancellationToken);
        }
    }

    private class RequestBodyDto
    {
        public List<Guid> OperationIds
        {
            get;
            set;
        } = default!;
    }
}
