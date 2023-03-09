using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class RunAsHandler<TBllContext> : BaseWriteHandler, IRunAsHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public RunAsHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var principal = await this.ParseRequestBodyAsync<string>(context);
            this._contextEvaluator.Evaluate(DBSessionMode.Write, x => x.Authorization.RunAsManager.StartRunAsUser(principal));
        }
    }
}
