using CommonFramework;

using Microsoft.AspNetCore.Http;

namespace Framework.Infrastructure.Auth;

public class ApplicationDefaultCancellationTokenSource(IHttpContextAccessor httpContextAccessor) : IDefaultCancellationTokenSource
{
    public CancellationToken CancellationToken => httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
}
