using System;
using Automation.ServiceEnvironment;
using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public abstract class WebApiBase : IRootServiceProviderContainer
{
    private readonly IServiceProvider serviceProvider;

    protected WebApiBase(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
            where TController : ControllerBase
    {
        return this.serviceProvider.GetDefaultControllerEvaluator<TController>(principalName);
    }

    IServiceProvider IRootServiceProviderContainer.RootServiceProvider => this.serviceProvider;
}
