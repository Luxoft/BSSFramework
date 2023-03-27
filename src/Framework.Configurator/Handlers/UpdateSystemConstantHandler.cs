using Framework.Configuration.BLL;
using Framework.Configurator.Interfaces;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record UpdateSystemConstantHandler(ISystemConstantBLLFactory SystemConstantBllFactory) : BaseWriteHandler,
    IUpdateSystemConstantHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var constantId = (string?)context.Request.RouteValues["id"] ?? throw new InvalidOperationException();
        var newValue = await this.ParseRequestBodyAsync<string>(context);

        this.Update(new Guid(constantId), newValue);
    }

    private void Update(Guid id, string newValue)
    {
        var systemConstantBll = this.SystemConstantBllFactory.Create(BLLSecurityMode.Edit);
        var systemConstant = systemConstantBll.GetById(id, true);
        systemConstant.Value = newValue;
        systemConstantBll.Save(systemConstant);
    }
}
