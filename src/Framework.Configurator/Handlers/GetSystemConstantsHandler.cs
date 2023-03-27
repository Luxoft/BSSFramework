using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetSystemConstantsHandler : BaseReadHandler, IGetSystemConstantsHandler
{
    private readonly ISystemConstantBLLFactory systemConstantBllFactory;

    public GetSystemConstantsHandler(ISystemConstantBLLFactory systemConstantBllFactory) =>
            this.systemConstantBllFactory = systemConstantBllFactory;

    protected override object GetData(HttpContext context) =>
            this.systemConstantBllFactory.Create(BLLSecurityMode.View)
                .GetSecureQueryable()
                .Select(
                        s => new SystemConstantDto { Id = s.Id, Name = s.Code, Description = s.Description, Value = s.Value })
                .OrderBy(s => s.Name)
                .ToList();
}
