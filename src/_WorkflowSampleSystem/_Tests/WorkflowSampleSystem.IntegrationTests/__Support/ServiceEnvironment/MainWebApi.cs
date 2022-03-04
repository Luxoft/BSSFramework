using System;

using WorkflowSampleSystem.WebApiCore.Controllers.Main;

namespace WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class MainWebApi : WebApiBase
{
    public MainWebApi(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public ControllerEvaluator<EmployeeController> Employee => this.GetControllerEvaluator<EmployeeController>();

    public ControllerEvaluator<LocationController> Location => this.GetControllerEvaluator<LocationController>();
}
