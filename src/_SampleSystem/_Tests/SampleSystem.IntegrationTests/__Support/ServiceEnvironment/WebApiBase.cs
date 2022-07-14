using System;

using Framework.DomainDriven.WebApiNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public abstract class WebApiBase : IRootServiceProviderContainer
{
    private readonly IServiceProvider serviceProvider;

    protected WebApiBase(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
            where TController : ControllerBase, IApiControllerBase
    {
        return this.serviceProvider.GetDefaultControllerEvaluator<TController>(principalName);
    }

    IServiceProvider IRootServiceProviderContainer.RootServiceProvider => this.serviceProvider;
}
