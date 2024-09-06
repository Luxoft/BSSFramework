using Framework.Configuration.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record UpdateSystemConstantHandler(IRepositoryFactory<SystemConstant> RepoFactory)
    : BaseWriteHandler, IUpdateSystemConstantHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var constantId = (string?)context.Request.RouteValues["id"]!;
        var newValue = await this.ParseRequestBodyAsync<string>(context);

        await this.UpdateAsync(new Guid(constantId), newValue, cancellationToken);
    }

    private async Task UpdateAsync(Guid id, string newValue, CancellationToken cancellationToken)
    {
        var systemConstant = await this.RepoFactory.Create().LoadAsync(id, cancellationToken);
        systemConstant.Value = newValue;
        await this.RepoFactory.Create(SecurityRole.Administrator).SaveAsync(systemConstant, cancellationToken);
    }
}
