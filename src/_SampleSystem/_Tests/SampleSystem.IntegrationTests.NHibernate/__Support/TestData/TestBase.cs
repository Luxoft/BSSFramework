using Framework.AutomationCore;
using Framework.AutomationCore.ServiceEnvironment;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.__Support.WebApi;
using SampleSystem.WebApiCore.Controllers.Main;
using SecuritySystem.Testing;

namespace SampleSystem.IntegrationTests.__Support.TestData;

public class TestBase : IntegrationTestBase<ISampleSystemBLLContext>, IDisposable
{
    protected TestBase() : base(InitializeAndCleanup.TestEnvironment.ServiceProviderPool)
    {
        InitializeAndCleanup.EnsureInitialized();
        base.Initialize();
    }

    public MainWebApi MainWebApi => new(this.RootServiceProvider);

    public MainAuditWebApi MainAuditWebApi => new(this.RootServiceProvider);

    protected DataHelper DataHelper => this.RootServiceProvider.GetRequiredService<DataHelper>();

    protected RootAuthManager AuthManager => this.RootServiceProvider.GetRequiredService<RootAuthManager>();

    public void Dispose()
    {
        this.BeforeCleanup();
        base.Cleanup();
    }

    protected virtual void BeforeCleanup()
    {
    }

    protected ControllerEvaluator<AuthMainController> GetAuthControllerEvaluator(string? principalName = null) => this.GetControllerEvaluator<AuthMainController>(principalName);

    protected ControllerEvaluator<ConfigMainController> GetConfigurationControllerEvaluator(string? principalName = null) => this.GetControllerEvaluator<ConfigMainController>(principalName);
}
