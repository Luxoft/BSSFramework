using Framework.Authorization.SecuritySystem;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetRunAsHandler(ICurrentPrincipalSource principalSource) : BaseReadHandler, IGetRunAsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken) =>
        principalSource.CurrentPrincipal.RunAs?.Name ?? string.Empty;
}
