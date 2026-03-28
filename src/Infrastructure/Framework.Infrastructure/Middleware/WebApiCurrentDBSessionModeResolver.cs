using CommonFramework;

using Framework.Database;

namespace Framework.Infrastructure.Middleware;

public class WebApiCurrentDBSessionModeResolver(IWebApiCurrentMethodResolver methodResolver, IWebApiDBSessionModeResolver dbSessionModeResolver)
    : IWebApiCurrentDBSessionModeResolver
{
    public DBSessionMode? GetSessionMode() => methodResolver.TryGetCurrentMethod().Maybe(dbSessionModeResolver.GetSessionMode);
}
