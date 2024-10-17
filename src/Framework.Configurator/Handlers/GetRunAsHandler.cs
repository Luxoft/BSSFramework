using Framework.Configurator.Interfaces;
using Framework.SecuritySystem.Services;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetRunAsHandler(IRunAsManager? runAsManager = null) : BaseReadHandler, IGetRunAsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken) =>
        runAsManager?.RunAsUser?.Name ?? string.Empty;
}
