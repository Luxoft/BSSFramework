using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.DbGenerate;
using Automation.Utils;

namespace WorkflowSampleSystem.IntegrationTests.DBGeneration
{
    [TestClass]
    public class SchemeUpdateTests : TestBase
    {
        [TestMethod]
        public void SchemeUpdate_ExistsDatabase_ShouldNotFail()
        {
            // Arrange
            var generator = new UseSchemeUpdateTest();

            // Act
            var action = new Action(() => UseSchemeUpdateTest.UseSchemeUpdate(AppSettings.Default["ConnectionStrings"]));

            // Assert
            action.Should().NotThrow();
        }
    }
}
