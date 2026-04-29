using Anch.SecuritySystem;

using Framework.AutomationCore.ServiceEnvironment;

using Microsoft.AspNetCore.Mvc;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public interface IRootServiceProviderContainer
{
    IServiceProvider RootServiceProvider { get; }

    ControllerEvaluator<TController> GetControllerEvaluator<TController>(UserCredential? userCredential = null)
        where TController : ControllerBase =>
        this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(userCredential);
}
