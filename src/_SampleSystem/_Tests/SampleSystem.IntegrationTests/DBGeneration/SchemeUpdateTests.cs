using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;
using Automation.Utils;
using Microsoft.Extensions.Configuration;

namespace SampleSystem.IntegrationTests.DBGeneration;

[TestClass]
public class SchemeUpdateTests : TestBase
{
    [TestMethod]
    public void SchemeUpdate_ExistsDatabase_ShouldNotFail()
    {
        // Arrange
        var generator = new UseSchemeUpdateTest();

        // Act
        var action = new Action(() => UseSchemeUpdateTest.UseSchemeUpdate(this.DatabaseContext.Main.ConnectionString));

        // Assert
        action.Should().NotThrow();
    }
}
