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
        return this._methodResolver
                   .CurrentMethod
                   .GetCustomAttribute<DBSessionModeAttribute>()
                   .ToMaybe()
                   .Select(attr => attr.SessionMode)
                   .ToNullable();
    }
}
