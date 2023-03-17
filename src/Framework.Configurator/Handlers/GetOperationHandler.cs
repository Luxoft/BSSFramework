using System;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class GetOperationHandler: BaseReadHandler, IGetOperationHandler
    {
        private readonly IAuthorizationBLLContext authorizationBllContext;

        public GetOperationHandler(IAuthorizationBLLContext authorizationBllContext) => this.authorizationBllContext = authorizationBllContext;

        protected override object GetData(HttpContext context)
        {
            var operationId = new Guid((string)context.Request.RouteValues["id"]);
            var businessRoles = this.authorizationBllContext.Authorization.Logics.BusinessRoleFactory.Create(BLLSecurityMode.View)
                                    .GetSecureQueryable()
                                    .Where(z => z.BusinessRoleOperationLinks.Any(link => link.Operation.Id == operationId))
                                    .Select(r => r.Name)
                                    .OrderBy(z => z)
                                    .Distinct()
                                    .ToList();
            
            var principals = this.authorizationBllContext.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.View)
                                 .GetSecureQueryable()
                                 .Where(p => p.Permissions.Any(z => z.Role.BusinessRoleOperationLinks.Any(link => link.Operation.Id == operationId)))
                                 .Select(p => p.Name)
                                 .OrderBy(p => p)
                                 .Distinct()
                                 .ToList();
            
            return new OperationDetailsDto { BusinessRoles = businessRoles, Principals = principals };
        }
    }
}
