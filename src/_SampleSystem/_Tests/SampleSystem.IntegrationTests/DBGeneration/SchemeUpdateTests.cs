using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;

namespace SampleSystem.IntegrationTests.DBGeneration
{
    [TestClass]
    public class SchemeUpdateTests : TestBase
    {
        [TestMethod]
        public void SchemeUpdate_ExistsDatabase_ShouldNotFail()
        {
            // Arrange
            var connectionString =
                    $"Data Source={this.DefaultDatabaseServer};Initial Catalog={this.DatabaseName};Integrated Security=True;Application Name=SampleSystem";
            var generator = new UseSchemeUpdateTest();

            // Act
            var action = new Action(() => UseSchemeUpdateTest.UseSchemeUpdate(connectionString));

            // Assert
            action.Should().NotThrow();
        }
    }
}
