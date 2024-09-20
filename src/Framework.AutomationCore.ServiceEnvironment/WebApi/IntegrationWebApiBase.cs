namespace Automation.ServiceEnvironment;

public abstract class IntegrationWebApiBase(IServiceProvider rootServiceProvider) : WebApiBase(rootServiceProvider)
{
    protected abstract string IntegrationUserName { get; }

    public override ControllerEvaluator<TController> GetControllerEvaluator<TController>(string? principalName = null) =>
            base.GetControllerEvaluator<TController>(principalName ?? this.IntegrationUserName);
}
