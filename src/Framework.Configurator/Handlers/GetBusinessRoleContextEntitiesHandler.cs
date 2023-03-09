using System;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers
{
    public class GetBusinessRoleContextEntitiesHandler<TBllContext> : BaseReadHandler, IGetBusinessRoleContextEntitiesHandler
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
    {
        private readonly IContextEvaluator<TBllContext> _contextEvaluator;

        public GetBusinessRoleContextEntitiesHandler(IContextEvaluator<TBllContext> contextEvaluator) =>
            this._contextEvaluator = contextEvaluator;

        protected override object GetData(HttpContext context)
        {
            var entityTypeId = new Guid((string)context.Request.RouteValues["id"]);
            var searchToken = context.Request.Query["searchToken"];

            return this._contextEvaluator.Evaluate(
                DBSessionMode.Read,
                x =>
                {
                    var entityType = x.Authorization.Logics.EntityTypeFactory.Create(BLLSecurityMode.View).GetById(entityTypeId, true);

                    var entities = x.Authorization.ExternalSource.GetTyped(entityType).GetSecurityEntities();
                    if (!string.IsNullOrWhiteSpace(searchToken))
                    {
                        entities = entities.Where(p => p.Name.Contains(searchToken, StringComparison.OrdinalIgnoreCase));
                    }

                    return entities
                           .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
                           .OrderBy(r => r.Name)
                           .Take(70)
                           .ToList();
                });
        }
    }
}
