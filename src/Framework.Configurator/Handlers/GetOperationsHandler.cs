using System.Linq;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class GetOperationsHandler<TBllContext> : BaseReadHandler, IGetOperationsHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetOperationsHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x => x.Authorization.Logics.OperationFactory.Create(BLLSecurityMode.View)
                      .GetSecureQueryable()
                      .Select(o => new OperationDto { Id = o.Id, Name = o.Name, Description = o.Description })
                      .OrderBy(o => o.Name)
                      .ToList());
    }
}
