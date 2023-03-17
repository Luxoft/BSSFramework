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
    public class DeleteBusinessRoleHandler: BaseWriteHandler, IDeleteBusinessRoleHandler
    {
        private readonly IAuthorizationBLLContext authorizationBllContext;

        public DeleteBusinessRoleHandler(IAuthorizationBLLContext authorizationBllContext) => this.authorizationBllContext = authorizationBllContext;
        

        public Task Execute(HttpContext context)
        {
            var roleId = new Guid((string)context.Request.RouteValues["id"]);
            this.Delete(roleId);

            return Task.CompletedTask;
        }

        private void Delete(Guid id)
        {
            var businessRoleBll = this.authorizationBllContext.Authorization.Logics
                                      .BusinessRoleFactory.Create(BLLSecurityMode.Edit);
            var domainObject = businessRoleBll.GetById(id, true);
            businessRoleBll.Remove(domainObject);
        }
    }
}
