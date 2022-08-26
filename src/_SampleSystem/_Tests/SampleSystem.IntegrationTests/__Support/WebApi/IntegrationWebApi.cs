using System;

using Automation.ServiceEnvironment;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

public class IntegrationWebApi : IntegrationWebApiBase
{
    public IntegrationWebApi(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override string IntegrationUserName { get; } = DefaultConstants.INTEGRATION_USER;
}
