using System;

namespace Automation.ServiceEnvironment;

public abstract class IntegrationWebApiBase : WebApiBase
{
    protected IntegrationWebApiBase(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
    {
    }

    protected abstract string IntegrationUserName { get; }

    public override ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null) =>
            base.GetControllerEvaluator<TController>(principalName ?? this.IntegrationUserName);
}
