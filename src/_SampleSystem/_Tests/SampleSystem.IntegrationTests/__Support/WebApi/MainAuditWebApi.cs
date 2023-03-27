using Automation.ServiceEnvironment;

using SampleSystem.WebApiCore.Controllers.Audit;

namespace SampleSystem.IntegrationTests;

public class MainAuditWebApi : WebApiBase
{
    public MainAuditWebApi(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public ControllerEvaluator<EmployeeController> Employee => this.GetControllerEvaluator<EmployeeController>();

    public ControllerEvaluator<LocationController> Location => this.GetControllerEvaluator<LocationController>();
}
