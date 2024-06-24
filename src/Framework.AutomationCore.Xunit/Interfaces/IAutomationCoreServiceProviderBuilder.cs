using System.Reflection;

namespace Automation.Xunit.Interfaces;

public interface IAutomationCoreServiceProviderBuilder
{
    public IServiceProvider GetFrameworkServiceProvider(AssemblyName assemblyName);
}
