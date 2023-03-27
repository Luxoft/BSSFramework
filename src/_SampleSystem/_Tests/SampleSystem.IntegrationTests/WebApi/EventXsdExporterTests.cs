using FluentAssertions;

using Framework.DomainDriven.WebApiNetCore.Integration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleSystem.IntegrationTests.WebApi;

[TestClass]
public class EventXsdExporterTests
{
    [TestMethod]
    public void WebApi_CallMethod()
    {
        // Arrange
        var exporter2 = new EventXsdExporter2();

        // Act
        var stream = exporter2.Export("Test", "test", new[] { typeof(TestDto) });

        // Assert
        stream.Should().BeReadable();
    }

    public class TestDto
    {
        public Guid[] Type { get; set; }
    }
}
