using Framework.Database;

namespace Framework.Infrastructure.Middleware;

public interface IWebApiDBSessionModeResolver
{
    DBSessionMode? GetSessionMode();
}
