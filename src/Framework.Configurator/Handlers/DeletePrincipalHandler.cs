using System;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class DeletePrincipalHandler<TBllContext> : BaseWriteHandler, IDeletePrincipalHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public DeletePrincipalHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public Task Execute(HttpContext context)
        {
            var principalId = new Guid((string)context.Request.RouteValues["id"]);
            this.Delete(principalId);

            return Task.CompletedTask;
        }

        private void Delete(Guid id) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    var domainObject = x.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit).GetById(id, true);
                    x.Authorization.Logics.Principal.Remove(domainObject);
                });
    }
}
