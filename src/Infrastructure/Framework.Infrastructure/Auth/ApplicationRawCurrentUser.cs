using CommonFramework.Auth;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Auth;

public class ApplicationRawCurrentUser([FromKeyedServices(ICurrentUser.DefaultKey)] ICurrentUser defaultCurrentUser) : ICurrentUser
{
    public string Name => defaultCurrentUser.Name;
}
