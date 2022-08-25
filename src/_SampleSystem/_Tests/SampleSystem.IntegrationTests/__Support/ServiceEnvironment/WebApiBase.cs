using System;
using Automation;
using Automation.ServiceEnvironment;
using Microsoft.AspNetCore.Mvc;
using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public abstract class WebApiBase : IntegrationTestContextEvaluator<ISampleSystemBLLContext>
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
