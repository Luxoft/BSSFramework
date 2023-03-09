using System.Linq;

using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class GetDomainTypesHandler<TBllContext> : BaseReadHandler, IGetDomainTypesHandler
        where TBllContext : DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetDomainTypesHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x => x.Configuration.Logics.DomainTypeFactory.Create(BLLSecurityMode.View)
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
}
