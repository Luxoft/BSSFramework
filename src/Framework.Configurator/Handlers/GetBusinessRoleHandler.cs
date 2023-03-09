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
    public class GetBusinessRoleHandler<TBllContext> : BaseReadHandler, IGetBusinessRoleHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetBusinessRoleHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context)
        {
            var roleId = new Guid((string)context.Request.RouteValues["id"]);

            return this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x => new BusinessRoleDetailsDto
                     {
                         Operations = x.Authorization.Logics.OperationFactory.Create(BLLSecurityMode.View)
                                       .GetSecureQueryable()
                                       .Where(z => z.Links.Any(o => o.BusinessRole.Id == roleId))
                                       .Select(o => new OperationDto { Id = o.Id, Name = o.Name, Description = o.Description })
                                       .OrderBy(o => o.Name)
                                       .Distinct()
                                       .ToList(),
                         Principals = x.Authorization.Logics.PermissionFactory.Create(BLLSecurityMode.View)
                                       .GetSecureQueryable()
                                       .Where(p => p.Role.Id == roleId)
                                       .Select(p => p.Principal.Name)
                                       .OrderBy(p => p)
                                       .Distinct()
                                       .ToList()
                     });
        }
    }
}
