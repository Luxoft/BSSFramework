using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetSystemConstantsHandler(IRepositoryFactory<SystemConstant> systemConstantRepositoryFactory)
    : BaseReadHandler, IGetSystemConstantsHandler
{
    protected override Task<object> GetData(HttpContext context) =>
        Task.FromResult<object>(
            systemConstantRepositoryFactory
                .Create(SecurityRule.View)
                .GetQueryable()
                .Select(s => new SystemConstantDto { Id = s.Id, Name = s.Code, Description = s.Description, Value = s.Value })
                .OrderBy(s => s.Name)
                .ToList());
}
