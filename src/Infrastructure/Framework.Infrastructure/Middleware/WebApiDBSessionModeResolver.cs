using System.Collections.Concurrent;
using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Database;

namespace Framework.Infrastructure.Middleware;

public class WebApiDBSessionModeResolver : IWebApiDBSessionModeResolver
{
    private readonly ConcurrentDictionary<MethodInfo, DBSessionMode?> cache = [];

    public DBSessionMode? GetSessionMode(MethodInfo methodInfo) =>
        this.cache.GetOrAdd(
            methodInfo,
            _ =>
            {
                var attrs = new[] { methodInfo.GetCustomAttribute<DBSessionModeAttribute>(), methodInfo.ReflectedType!.GetCustomAttribute<DBSessionModeAttribute>() };

                return attrs.FirstOrDefault(attr => attr != null)
                            .ToMaybe()
                            .Select(attr => attr.SessionMode)
                            .ToNullable();
            });
}
