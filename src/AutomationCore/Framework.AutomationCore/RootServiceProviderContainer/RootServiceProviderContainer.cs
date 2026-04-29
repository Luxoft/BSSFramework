using Anch.SecuritySystem;

using Framework.AutomationCore.ServiceEnvironment;

using Microsoft.AspNetCore.Mvc;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public class RootServiceProviderContainer(IServiceProvider rootServiceProvider) : IRootServiceProviderContainer
{
    public IServiceProvider RootServiceProvider { get; } = rootServiceProvider;

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(UserCredential? userCredential = null)
        where TController : ControllerBase =>
        this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(userCredential);
}
