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
    public class UpdatePrincipalHandler<TBllContext> : BaseWriteHandler, IUpdatePrincipalHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public UpdatePrincipalHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var principalId = (string)context.Request.RouteValues["id"];
            var name = await this.ParseRequestBodyAsync<string>(context);

            this.Update(new Guid(principalId), name);
        }

        private void Update(Guid id, string newName) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    var domainObject = x.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit).GetById(id, true);
                    domainObject.Name = newName;
                    x.Authorization.Logics.Principal.Save(domainObject);
                });
    }
}
