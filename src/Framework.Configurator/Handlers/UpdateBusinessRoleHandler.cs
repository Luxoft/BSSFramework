using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class UpdateBusinessRoleHandler<TBllContext> : BaseWriteHandler, IUpdateBusinessRoleHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public UpdateBusinessRoleHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        public async Task Execute(HttpContext context)
        {
            var roleId = (string)context.Request.RouteValues["id"];
            var role = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

            this.Update(new Guid(roleId), role);
        }

        private void Update(Guid id, RequestBodyDto role) =>
            this._contextEvaluator.Evaluate(
                DBSessionMode.Write,
                x =>
                {
                    var domainObject = x.Authorization.Logics.BusinessRoleFactory.Create(BLLSecurityMode.Edit).GetById(id, true);
                    var mergeResult = domainObject.BusinessRoleOperationLinks.GetMergeResult(
                        role.OperationIds,
                        s => s.Operation.Id,
                        s => s);

                    foreach (var operation in x.Authorization.Logics.Operation.GetListByIdents(mergeResult.AddingItems))
                    {
                        new BusinessRoleOperationLink(domainObject) { Operation = operation };
                    }

                    domainObject.RemoveDetails(mergeResult.RemovingItems);
                    x.Authorization.Logics.BusinessRole.Save(domainObject);
                });

        private class RequestBodyDto
        {
            public List<Guid> OperationIds
            {
                get;
                set;
            }
        }
    }
}
