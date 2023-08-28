namespace Framework.DomainDriven.WebApiNetCore;

public interface IWebApiDBSessionModeResolver
{
    DBSessionMode? GetSessionMode();
}
