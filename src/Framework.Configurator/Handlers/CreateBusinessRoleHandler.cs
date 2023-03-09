using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class CreateBusinessRoleHandler<TBllContext> : BaseWriteHandler, ICreateBusinessRoleHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public CreateBusinessRoleHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var newRole = await this.ParseRequestBodyAsync<RequestBodyDto>(context);
            this.Create(newRole);
        }

        private void Create(RequestBodyDto newRole) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    var domainObject = new BusinessRole { Name = newRole.Name };
                    foreach (var operation in x.Authorization.Logics.Operation.GetListByIdents(newRole.OperationIds))
                    {
                        new BusinessRoleOperationLink(domainObject) { Operation = operation };
                    }

                    x.Authorization.Logics.BusinessRoleFactory.Create(BLLSecurityMode.Edit).Save(domainObject);
                });

        private class RequestBodyDto
        {
            public List<Guid> OperationIds
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }
        }
    }
}
