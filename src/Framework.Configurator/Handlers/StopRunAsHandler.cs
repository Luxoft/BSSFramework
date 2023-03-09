using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class StopRunAsHandler<TBllContext> : BaseWriteHandler, IStopRunAsHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public StopRunAsHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public Task Execute(HttpContext context)
        {
            this._contextEvaluator.Evaluate(DBSessionMode.Write, x => x.Authorization.RunAsManager.FinishRunAsUser());
            return Task.CompletedTask;
        }
    }
}
