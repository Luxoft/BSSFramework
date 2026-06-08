using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.WebApi;

public partial class AuthMainController
{
    [HttpPost]
    public async Task RunAsUser([FromForm] PrincipalIdentityDTO principal, CancellationToken ct)
    {
        var context = this.HttpContext.RequestServices.GetRequiredService<IAuthorizationBLLContext>();

        await context.RunAsManager.StartRunAsUserAsync(principal.Id, ct);
    }

    [HttpPost]
    public async Task FinishRunAsUser(CancellationToken ct)
    {
        var context = this.HttpContext.RequestServices.GetRequiredService<IAuthorizationBLLContext>();

        await context.RunAsManager.FinishRunAsUserAsync(ct);
    }
}
