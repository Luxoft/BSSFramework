using System;

using Automation.ServiceEnvironment;

using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class MainWebApi : WebApiBase
{
    public MainWebApi(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public ControllerEvaluator<EmployeeController> Employee => this.GetControllerEvaluator<EmployeeController>();

    public ControllerEvaluator<LocationController> Location => this.GetControllerEvaluator<LocationController>();

    public ControllerEvaluator<CountryController> Country => this.GetControllerEvaluator<CountryController>();
}
