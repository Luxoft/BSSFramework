using System.Reflection;

using Framework.DomainDriven.WebApiNetCore;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class IntegrationTestsWebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    public MethodInfo CurrentMethod { get; set; }
}
