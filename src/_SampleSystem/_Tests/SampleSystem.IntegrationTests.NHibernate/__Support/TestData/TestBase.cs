using Automation.ServiceEnvironment;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.WebApiCore.Controllers;

namespace SampleSystem.IntegrationTests.__Support.TestData;

[TestClass]
public class TestBase : IntegrationTestBase<ISampleSystemBLLContext>
{
    protected TestBase() : base(InitializeAndCleanup.TestEnvironment.ServiceProviderPool)
    {
    }

    public MainWebApi MainWebApi => new(this.RootServiceProvider);

    public MainAuditWebApi MainAuditWebApi => new(this.RootServiceProvider);

    protected DataHelper DataHelper => this.RootServiceProvider.GetService<DataHelper>();

    protected RootAuthManager AuthManager => this.RootServiceProvider.GetService<RootAuthManager>();

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

    protected ControllerEvaluator<AuthSLJsonController> GetAuthControllerEvaluator(string principalName = null)
    {
        return this.GetControllerEvaluator<AuthSLJsonController>(principalName);
    }

    protected ControllerEvaluator<ConfigSLJsonController> GetConfigurationControllerEvaluator(string principalName = null)
    {
        return this.GetControllerEvaluator<ConfigSLJsonController>(principalName);
    }
}
