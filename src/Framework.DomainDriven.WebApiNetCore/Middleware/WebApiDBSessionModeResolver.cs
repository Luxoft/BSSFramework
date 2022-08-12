using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiDBSessionModeResolver: IWebApiDBSessionModeResolver
{
    private readonly IWebApiCurrentMethodResolver _methodResolver;

    public WebApiDBSessionModeResolver(IWebApiCurrentMethodResolver methodResolver)
    {
        this._methodResolver = methodResolver;
    }

    public DBSessionMode? GetSessionMode()
    {
        var attrs = new[]
        {
            this._methodResolver.CurrentMethod.GetCustomAttribute<DBSessionModeAttribute>(),
            this._methodResolver.CurrentMethod.ReflectedType.GetCustomAttribute<DBSessionModeAttribute>()
        };

        return attrs.FirstOrDefault(attr => attr != null)
                    .ToMaybe()
                    .Select(attr => attr.SessionMode)
                    .ToNullable();
    }
}
