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
    public class GetPrincipalsHandler<TBllContext> : BaseReadHandler, IGetPrincipalsHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetPrincipalsHandler(IContextEvaluator<TBllContext> contextEvaluator) => this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context)
        {
            var searchToken = context.Request.Query["searchToken"];

            return this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x =>
                {
                    var query = x.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.View).GetSecureQueryable();
                    if (!string.IsNullOrWhiteSpace(searchToken))
                    {
                        query = query.Where(p => p.Name.Contains(searchToken));
                    }

                    return query
                           .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
                           .OrderBy(r => r.Name)
                           .Take(70)
                           .ToList();
                });
        }
    }
}
