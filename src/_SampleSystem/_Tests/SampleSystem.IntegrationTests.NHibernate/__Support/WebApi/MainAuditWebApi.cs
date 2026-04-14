using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.WebApi;

using SampleSystem.WebApiCore.Controllers.Audit;

namespace SampleSystem.IntegrationTests.__Support.WebApi;

public class MainAuditWebApi(IServiceProvider serviceProvider) : WebApiBase(serviceProvider)
{
    public ControllerEvaluator<EmployeeController> Employee => this.GetControllerEvaluator<EmployeeController>();

    public ControllerEvaluator<LocationController> Location => this.GetControllerEvaluator<LocationController>();
}
