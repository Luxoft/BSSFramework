using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record UploadPermissionHandler : IUploadPermissionHandler
{
    public Task Execute(HttpContext context, CancellationToken cancellationToken) => throw new NotImplementedException();
}
