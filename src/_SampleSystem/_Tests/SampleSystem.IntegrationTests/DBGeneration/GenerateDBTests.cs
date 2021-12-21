using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;

namespace SampleSystem.IntegrationTests.DBGeneration
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
            var action = new Action(() => generator.GenerateAllDB(this.DefaultDatabaseServer, this.DatabaseName));

            // Assert
            action.Should().NotThrow();
        }
    }
}
