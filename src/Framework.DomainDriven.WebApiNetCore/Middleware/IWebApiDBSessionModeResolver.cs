using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.WebApiNetCore;

public interface IWebApiDBSessionModeResolver
{
    DBSessionMode? GetSessionMode();
}
