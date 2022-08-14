using System.Reflection;

namespace Framework.DomainDriven.WebApiNetCore;

public interface IWebApiCurrentMethodResolver
{
    MethodInfo GetCurrentMethod();
}
