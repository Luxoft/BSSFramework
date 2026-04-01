using Framework.AutomationCore.ServiceEnvironment.WebApi;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.__Support.WebApi;

public class IntegrationWebApi(IServiceProvider serviceProvider) : IntegrationWebApiBase(serviceProvider)
{
    protected override string IntegrationUserName { get; } = DefaultConstants.INTEGRATION_BUS;
}
