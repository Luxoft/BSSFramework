using System.Linq;

using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.BLL;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class GetSystemConstantsHandler<TBllContext> : BaseReadHandler, IGetSystemConstantsHandler
        where TBllContext : DomainDriven.BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetSystemConstantsHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x => x.Configuration.Logics.SystemConstantFactory.Create(BLLSecurityMode.View)
                      .GetSecureQueryable()
                      .Select(s => new SystemConstantDto { Id = s.Id, Name = s.Code, Description = s.Description, Value = s.Value })
                      .OrderBy(s => s.Name)
                      .ToList());
    }
}
