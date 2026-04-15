using Framework.AutomationCore.ServiceEnvironment;

using SecuritySystem;

namespace Framework.AutomationCore.WebApi;

public abstract class IntegrationWebApiBase(IServiceProvider rootServiceProvider) : WebApiBase(rootServiceProvider)
{
    protected abstract string IntegrationUserName { get; }

    public override ControllerEvaluator<TController> GetControllerEvaluator<TController>(UserCredential? userCredential = null) =>
            base.GetControllerEvaluator<TController>(userCredential ?? this.IntegrationUserName);
}
