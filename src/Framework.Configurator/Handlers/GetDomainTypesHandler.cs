using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetDomainTypesHandler : BaseReadHandler, IGetDomainTypesHandler
{
    private readonly IDomainTypeBLLFactory domainTypeBllFactory;

    public GetDomainTypesHandler(IDomainTypeBLLFactory domainTypeBllFactory) => this.domainTypeBllFactory = domainTypeBllFactory;

    protected override object GetData(HttpContext context) =>
            this.domainTypeBllFactory.Create(SecurityRule.View)
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
                .ToList();
}
