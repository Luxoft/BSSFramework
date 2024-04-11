using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetDomainTypesHandler(IDomainTypeBLLFactory domainTypeBllFactory) : BaseReadHandler, IGetDomainTypesHandler
{
    protected override Task<object> GetData(HttpContext context) =>
        Task.FromResult<object>(
            domainTypeBllFactory.Create(SecurityRule.View)
                                .GetSecureQueryable()
                                .Where(d => d.TargetSystem.IsRevision)
                                .OrderBy(d => d.Name)
                                .Select(
                                    d => new DomainTypeDto
                                         {
                                             Id = d.Id,
                                             Name = d.Name,
                                             Namespace = d.NameSpace,
                                             Operations = d.EventOperations
                                                           .OrderBy(o => o.Name)
                                                           .Select(o => new EntityDto { Id = o.Id, Name = o.Name })
                                                           .ToList()
                                         })
                                .ToList());
}
