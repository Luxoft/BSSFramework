using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;
using Framework.DomainDriven.DBGenerator;

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
            var action = new Action(() => generator.GenerateAllDB(
                this.DatabaseContext.Main.DataSource,
                this.DatabaseContext.Main.DatabaseName,
                credential: UserCredential.Create(
                    this.DatabaseContext.Main.UserId,
                    this.DatabaseContext.Main.Password)));

            // Assert
            action.Should().NotThrow();
        }
    }
}
