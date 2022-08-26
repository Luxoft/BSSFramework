using System;

using Microsoft.AspNetCore.Mvc;

namespace Automation.ServiceEnvironment;

public abstract class WebApiBase : RootServiceProviderContainer
{
    protected WebApiBase(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
    {
    }

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
            where TController : ControllerBase
    {
        return this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(principalName);
    }
}
