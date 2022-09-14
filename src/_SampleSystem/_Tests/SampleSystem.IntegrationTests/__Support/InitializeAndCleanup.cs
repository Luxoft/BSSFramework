using Microsoft.VisualStudio.TestTools.UnitTesting;
using Automation;

namespace SampleSystem.IntegrationTests.__Support
{
    [TestClass]
    public class InitializeAndCleanup
    {
        private static readonly TestEnvironment TestEnvironment = SampleSystemTestEnvironment.Current;

        [AssemblyInitialize]
        public static void EnvironmentInitialize(TestContext testContext)
        {
            TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentInitialize();
        }

        [AssemblyCleanup]
        public static void EnvironmentCleanup()
        {
            TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentCleanup();
        }
    }
}
