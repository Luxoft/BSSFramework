using Automation;
using Automation.ServiceEnvironment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.WebApiCore.Controllers;
using DataHelper = SampleSystem.IntegrationTests.__Support.TestData.Helpers.DataHelper;

[assembly: Parallelize(Scope = ExecutionScope.ClassLevel)]
namespace SampleSystem.IntegrationTests.__Support.TestData
{
    [TestClass]
    public class TestBase : IntegrationTestBase<ISampleSystemBLLContext>
    {
        protected TestBase() : base(SampleSystemTestEnvironment.Current.ServiceProviderPool)
        {
            // Workaround for System.Drawing.Common problem https://chowdera.com/2021/12/202112240234238356.html
            System.AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
        }

        public MainWebApi MainWebApi => new(this.RootServiceProvider);

        public MainAuditWebApi MainAuditWebApi => new(this.RootServiceProvider);

        protected DataHelper DataHelper => this.RootServiceProvider.GetService<DataHelper>();

        protected AuthHelper AuthHelper => this.RootServiceProvider.GetService<AuthHelper>();

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
}
