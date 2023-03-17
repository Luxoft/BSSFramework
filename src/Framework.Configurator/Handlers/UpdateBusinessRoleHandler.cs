using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class UpdateBusinessRoleHandler: BaseWriteHandler, IUpdateBusinessRoleHandler
    {
        private readonly IAuthorizationBLLContext authorizationBllContext;

        public UpdateBusinessRoleHandler(IAuthorizationBLLContext authorizationBllContext) => this.authorizationBllContext = authorizationBllContext;

        public async Task Execute(HttpContext context)
        {
            var roleId = (string)context.Request.RouteValues["id"];
            var role = await this.ParseRequestBodyAsync<RequestBodyDto>(context);

            this.Update(new Guid(roleId), role);
        }

        [SuppressMessage("SonarQube", "S2436", Justification = "It's ok. BusinessRoleOperationLink automatically link to BusinessRole")]
        private void Update(Guid id, RequestBodyDto role)
        {
            var businessRoleBll = this.authorizationBllContext.Authorization.Logics
                                      .BusinessRoleFactory.Create(BLLSecurityMode.Edit);
            
            var domainObject = businessRoleBll.GetById(id, true);
            var mergeResult =
                    domainObject.BusinessRoleOperationLinks.GetMergeResult(
                     role.OperationIds,
                     s => s.Operation.Id,
                     s => s);

            var operations = this.authorizationBllContext.Authorization.Logics.Operation.GetListByIdents(mergeResult.AddingItems);
            
            foreach (var operation in operations)
            {
                new BusinessRoleOperationLink(domainObject) { Operation = operation };
            }

            domainObject.RemoveDetails(mergeResult.RemovingItems);
            businessRoleBll.Save(domainObject);
        }

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
