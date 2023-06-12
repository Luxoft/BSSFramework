using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetSystemConstantsHandler : BaseReadHandler, IGetSystemConstantsHandler
{
    private readonly IDefaultRepositoryFactory<SystemConstant> systemConstantRepositoryFactory;

    public GetSystemConstantsHandler(IDefaultRepositoryFactory<SystemConstant> systemConstantRepositoryFactory) =>
        this.systemConstantRepositoryFactory = systemConstantRepositoryFactory;

    protected override object GetData(HttpContext context) =>
        this.systemConstantRepositoryFactory.Create(BLLSecurityMode.View)
            .GetQueryable()
            .Select(
                s => new SystemConstantDto { Id = s.Id, Name = s.Code, Description = s.Description, Value = s.Value })
            .OrderBy(s => s.Name)
            .ToList();
}
