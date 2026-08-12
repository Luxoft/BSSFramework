using Framework.Database;

namespace Framework.Infrastructure.Middleware;

public interface IWebApiCurrentDBSessionModeResolver
{
    DBSessionMode? GetSessionMode();
}
