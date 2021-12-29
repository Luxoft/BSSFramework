using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;
using Automation.Utils;

namespace SampleSystem.IntegrationTests.DBGeneration
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
