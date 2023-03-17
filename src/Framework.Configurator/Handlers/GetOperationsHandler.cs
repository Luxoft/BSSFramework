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
    public class GetOperationsHandler: BaseReadHandler, IGetOperationsHandler
       
    {
        private readonly IAuthorizationBLLContext authorizationBllContext;

        public GetOperationsHandler(IAuthorizationBLLContext authorizationBllContext) => this.authorizationBllContext = authorizationBllContext;
        
        protected override object GetData(HttpContext context)
            => this.authorizationBllContext.Authorization.Logics.OperationFactory.Create(BLLSecurityMode.View)
                   .GetSecureQueryable()
                   .Select(o => new OperationDto { Id = o.Id, Name = o.Name, Description = o.Description })
                   .OrderBy(o => o.Name)
                   .ToList();
    }
}
