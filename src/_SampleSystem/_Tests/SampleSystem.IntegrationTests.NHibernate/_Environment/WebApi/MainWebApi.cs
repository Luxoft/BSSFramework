using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.WebApi;

using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests._Environment.WebApi;

public class MainWebApi(IServiceProvider serviceProvider) : WebApiBase(serviceProvider)
{
    public ControllerEvaluator<EmployeeController> Employee => this.GetControllerEvaluator<EmployeeController>();

    public ControllerEvaluator<LocationController> Location => this.GetControllerEvaluator<LocationController>();

    public ControllerEvaluator<CountryController> Country => this.GetControllerEvaluator<CountryController>();
}
