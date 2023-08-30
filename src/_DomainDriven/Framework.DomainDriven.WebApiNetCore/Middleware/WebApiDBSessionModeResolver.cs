using System.Reflection;

using Framework.Core;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiDBSessionModeResolver : IWebApiDBSessionModeResolver
{
    private readonly IWebApiCurrentMethodResolver methodResolver;

    public WebApiDBSessionModeResolver(IWebApiCurrentMethodResolver methodResolver)
    {
        this.methodResolver = methodResolver;
    }

    public DBSessionMode? GetSessionMode()
    {
        var currentMethod = this.methodResolver.GetCurrentMethod();

        var attrs = new[]
        {
            currentMethod.Maybe(m => m.GetCustomAttribute<DBSessionModeAttribute>()),
            currentMethod.Maybe(m => m.ReflectedType.GetCustomAttribute<DBSessionModeAttribute>())
        };

        return attrs.FirstOrDefault(attr => attr != null)
                    .ToMaybe()
                    .Select(attr => attr.SessionMode)
                    .ToNullable();
    }
}
