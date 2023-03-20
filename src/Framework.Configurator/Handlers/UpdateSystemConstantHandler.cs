using System;
using System.Threading.Tasks;

using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class UpdateSystemConstantHandler : BaseWriteHandler, IUpdateSystemConstantHandler
{
    private readonly ISystemConstantBLLFactory systemConstantBllFactory;

    public UpdateSystemConstantHandler(ISystemConstantBLLFactory systemConstantBllFactory) =>
            this.systemConstantBllFactory = systemConstantBllFactory;

    public async Task Execute(HttpContext context)
    {
        var constantId = (string)context.Request.RouteValues["id"] ?? throw new InvalidOperationException();
        var newValue = await this.ParseRequestBodyAsync<string>(context);

        this.Update(new Guid(constantId), newValue);
    }

    private void Update(Guid id, string newValue)
    {
        var systemConstantBll = this.systemConstantBllFactory.Create(BLLSecurityMode.Edit);
        var systemConstant = systemConstantBll.GetById(id, true);
        systemConstant.Value = newValue;
        systemConstantBll.Save(systemConstant);
    }
}
