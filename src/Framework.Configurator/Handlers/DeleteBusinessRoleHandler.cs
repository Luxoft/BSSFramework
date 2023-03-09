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
    public class DeleteBusinessRoleHandler<TBllContext> : BaseWriteHandler, IDeleteBusinessRoleHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public DeleteBusinessRoleHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public Task Execute(HttpContext context)
        {
            var roleId = new Guid((string)context.Request.RouteValues["id"]);
            this.Delete(roleId);

            return Task.CompletedTask;
        }

        private void Delete(Guid id) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    var domainObject = x.Authorization.Logics.BusinessRoleFactory.Create(BLLSecurityMode.Edit).GetById(id, true);
                    x.Authorization.Logics.BusinessRole.Remove(domainObject);
                });
    }
}
