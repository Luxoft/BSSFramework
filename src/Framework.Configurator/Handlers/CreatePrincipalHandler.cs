using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;






namespace Framework.Configurator.Handlers
{
    public class CreatePrincipalHandler<TBllContext> : BaseWriteHandler, ICreatePrincipalHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public CreatePrincipalHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var name = await this.ParseRequestBodyAsync<string>(context);

            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x => x.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit).Save(new Principal { Name = name }));
        }
    }
}
