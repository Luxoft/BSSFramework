using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.Xunit.Interfaces;

public interface IAutomationCoreInitialization
{
    public IServiceCollection ConfigureFramework(IServiceCollection services);

    public IServiceProvider ConfigureTestEnvironment(IServiceCollection services, IConfiguration configuration);
}
