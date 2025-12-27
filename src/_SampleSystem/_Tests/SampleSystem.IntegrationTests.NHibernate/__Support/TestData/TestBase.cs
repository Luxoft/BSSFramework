using Automation.ServiceEnvironment;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.__Support.WebApi;
using SampleSystem.WebApiCore.Controllers;

using SecuritySystem.Testing;

namespace SampleSystem.IntegrationTests.__Support.TestData;

[TestClass]
public class TestBase : IntegrationTestBase<ISampleSystemBLLContext>
{
    protected TestBase() : base(InitializeAndCleanup.TestEnvironment.ServiceProviderPool)
    {
    }

    public MainWebApi MainWebApi => new(this.RootServiceProvider);

    public MainAuditWebApi MainAuditWebApi => new(this.RootServiceProvider);

    protected DataHelper DataHelper => this.RootServiceProvider.GetRequiredService<DataHelper>();

    protected RootAuthManager AuthManager => this.RootServiceProvider.GetRequiredService<RootAuthManager>();

    [TestInitialize]
    public void TestBaseInitialize()
    {
        base.Initialize();
    }

    [TestCleanup]
    public void BaseTestCleanup()
    {
        base.Cleanup();
    }

    protected ControllerEvaluator<AuthSLJsonController> GetAuthControllerEvaluator(string? principalName = null)
    {
        return this.GetControllerEvaluator<AuthSLJsonController>(principalName);
    }

    protected ControllerEvaluator<ConfigSLJsonController> GetConfigurationControllerEvaluator(string? principalName = null)
    {
        return this.GetControllerEvaluator<ConfigSLJsonController>(principalName);
    }
}
