using System;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class DeletePrincipalHandler: BaseWriteHandler, IDeletePrincipalHandler
    {
        private readonly IAuthorizationBLLContext authorizationBllContext;
        public DeletePrincipalHandler(IAuthorizationBLLContext authorizationBllContext) => this.authorizationBllContext = authorizationBllContext;

        public Task Execute(HttpContext context)
        {
            var principalId = new Guid((string)context.Request.RouteValues["id"]);
            this.Delete(principalId);

            return Task.CompletedTask;
        }

        private void Delete(Guid id)
        {
            var principalBll = this.authorizationBllContext.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit);
            var domainObject = principalBll.GetById(id, true);
            principalBll.Remove(domainObject);
        }
    }
}
