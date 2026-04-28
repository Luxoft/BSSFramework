using Framework.AutomationCore.WebApi;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests._Environment.WebApi;

public class IntegrationWebApi(IServiceProvider serviceProvider) : IntegrationWebApiBase(serviceProvider)
{
    protected override string IntegrationUserName { get; } = DefaultConstants.INTEGRATION_BUS;
}
