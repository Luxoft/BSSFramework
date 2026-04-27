using Anch.SecuritySystem.Testing;

using Framework.AutomationCore;
using Framework.AutomationCore.ServiceEnvironment;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.__Support.WebApi;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests.__Support.TestData;

public class TestBase(IServiceProvider rootServiceProvider) : IntegrationTestBase<ISampleSystemBLLContext>(rootServiceProvider), IAsyncLifetime
{
    public MainWebApi MainWebApi => new(this.RootServiceProvider);

    public MainAuditWebApi MainAuditWebApi => new(this.RootServiceProvider);

    protected DataHelper DataHelper => this.RootServiceProvider.GetRequiredService<DataHelper>();

    protected RootAuthManager AuthManager => this.RootServiceProvider.GetRequiredService<RootAuthManager>();

    protected ControllerEvaluator<AuthMainController> GetAuthControllerEvaluator(string? principalName = null) =>
        this.GetControllerEvaluator<AuthMainController>(principalName);

    protected ControllerEvaluator<ConfigMainController> GetConfigurationControllerEvaluator(string? principalName = null) =>
        this.GetControllerEvaluator<ConfigMainController>(principalName);

    protected virtual ValueTask InitializeAsync(CancellationToken ct) => ValueTask.CompletedTask;

    ValueTask IAsyncLifetime.InitializeAsync() => this.InitializeAsync(TestContext.Current.CancellationToken);

    ValueTask IAsyncDisposable.DisposeAsync() => ValueTask.CompletedTask;
}
