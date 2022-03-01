using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.DbGenerate;
using Framework.DomainDriven.DBGenerator;
using WorkflowSampleSystem.IntegrationTests.__Support;

namespace WorkflowSampleSystem.IntegrationTests.DBGeneration
{
    [TestClass]
    public class GenerateDBTests : TestBase
    {
        [TestMethod]
        public void GenerateDB_SecondTime_ShouldNotFail()
        {
            // Arrange
            var generator = new DbGeneratorTest();

            // Act
            var action = new Action(() => generator.GenerateAllDB(
                                                InitializeAndCleanup.DatabaseUtil.ConnectionSettings.DataSource,
                                                credential: UserCredential.Create(InitializeAndCleanup.DatabaseUtil.ConnectionSettings.UserId, InitializeAndCleanup.DatabaseUtil.ConnectionSettings.Password)));

            // Assert
            action.Should().NotThrow();
        }
    }
}
