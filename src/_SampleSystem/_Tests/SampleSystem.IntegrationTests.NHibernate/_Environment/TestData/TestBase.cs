using Anch.SecuritySystem.Testing;

using Framework.AutomationCore;
using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.Utils.DatabaseUtils;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests._Environment.TestData.Helpers;
using SampleSystem.IntegrationTests._Environment.WebApi;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests._Environment.TestData;

public class TestBase(IServiceProvider rootServiceProvider) : IntegrationTestBase<ISampleSystemBLLContext>(rootServiceProvider), IAsyncLifetime
{
    protected MainWebApi MainWebApi => new(this.RootServiceProvider);

    protected MainAuditWebApi MainAuditWebApi => new(this.RootServiceProvider);

    protected IDatabaseContext DatabaseContext => this.RootServiceProvider.GetRequiredService<IDatabaseContext>();

    protected DataManager DataManager => this.RootServiceProvider.GetRequiredService<DataManager>();

    protected RootAuthManager AuthManager => this.RootServiceProvider.GetRequiredService<RootAuthManager>();

    protected ControllerEvaluator<AuthMainController> GetAuthControllerEvaluator(string? principalName = null) =>
        this.GetControllerEvaluator<AuthMainController>(principalName);

    protected ControllerEvaluator<ConfigMainController> GetConfigurationControllerEvaluator(string? principalName = null) =>
        this.GetControllerEvaluator<ConfigMainController>(principalName);

    protected virtual ValueTask InitializeAsync(CancellationToken ct) => ValueTask.CompletedTask;

    ValueTask IAsyncLifetime.InitializeAsync() => this.InitializeAsync(TestContext.Current.CancellationToken);

    ValueTask IAsyncDisposable.DisposeAsync() => ValueTask.CompletedTask;
}
