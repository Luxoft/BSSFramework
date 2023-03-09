using System.Linq;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class GetBusinessRoleContextsHandler<TBllContext> : BaseReadHandler, IGetBusinessRoleContextsHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetBusinessRoleContextsHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x => x.Authorization.Logics.EntityTypeFactory.Create(BLLSecurityMode.View)
                      .GetSecureQueryable()
                      .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
                      .OrderBy(r => r.Name)
                      .ToList());
    }
}
