using Microsoft.VisualStudio.TestTools.UnitTesting;

using Automation;
using Automation.Utils;

using AttachmentsSampleSystem.IntegrationTests.Support.Utils;

namespace AttachmentsSampleSystem.IntegrationTests.__Support
{
    [TestClass]
    public class InitializeAndCleanup
    {
        public static AttachmentsSampleSystemDatabaseUtil DatabaseUtil { get; set; }

        [AssemblyInitialize]
        public static void EnvironmentInitialize(TestContext testContext)
        {
            AppSettings.Initialize(nameof(AttachmentsSampleSystem) + "_");

            DatabaseUtil = new AttachmentsSampleSystemDatabaseUtil();

            AssemblyInitializeAndCleanup.EnvironmentInitialize(DatabaseUtil);
        }

        [AssemblyCleanup]
        public static void EnvironmentCleanup()
        {
            AssemblyInitializeAndCleanup.EnvironmentCleanup(DatabaseUtil);
        }
    }
}
