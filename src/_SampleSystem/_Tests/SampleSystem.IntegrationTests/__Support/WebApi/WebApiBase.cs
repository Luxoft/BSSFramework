using System;

using Automation.ServiceEnvironment;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests;

public abstract class WebApiBase : RootServiceProviderContainer<ISampleSystemBLLContext>
{
    protected WebApiBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
            where TController : ControllerBase
    {
        return this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(principalName);
    }
}
