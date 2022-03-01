using Microsoft.Data.SqlClient;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.Utils;
using SampleSystem.IntegrationTests.__Support.Utils.Framework;

using Automation;
using Automation.Utils;

using Microsoft.Extensions.Configuration;

using SampleSystem.IntegrationTests.Support.Utils;

namespace SampleSystem.IntegrationTests.__Support
{
    [TestClass]
    public class InitializeAndCleanup
    {
        public static SampleSystemDatabaseUtil DatabaseUtil { get; set; }

        [AssemblyInitialize]
        public static void EnvironmentInitialize(TestContext testContext)
        {
            AppSettings.Initialize(nameof(SampleSystem) + "_");

            DatabaseUtil = new SampleSystemDatabaseUtil();

            AssemblyInitializeAndCleanup.EnvironmentInitialize(DatabaseUtil);
        }

        [AssemblyCleanup]
        public static void EnvironmentCleanup()
        {
            AssemblyInitializeAndCleanup.EnvironmentCleanup(DatabaseUtil);
        }
    }
}
