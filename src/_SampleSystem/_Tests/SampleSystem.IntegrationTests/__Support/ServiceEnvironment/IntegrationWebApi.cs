using System;
using Automation.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class IntegrationWebApi : WebApiBase
{
    public IntegrationWebApi(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public override ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null) =>
            base.GetControllerEvaluator<TController>(principalName).WithImpersonate(DefaultConstants.INTEGRATION_USER);
}
