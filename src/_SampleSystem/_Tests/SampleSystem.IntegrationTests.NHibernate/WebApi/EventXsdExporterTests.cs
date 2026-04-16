using Framework.Infrastructure.Integration;

namespace SampleSystem.IntegrationTests.WebApi;

public class EventXsdExporterTests
{
    [Fact]
    public void WebApi_CallMethod()
    {
        // Arrange
        var exporter2 = new EventXsdExporter2();

        // Act
        var stream = exporter2.Export("Test", "test", [typeof(TestDto)]);

        // Assert
        Assert.True(stream.CanRead);
    }

    public class TestDto
    {
        public Guid[] Type { get; set; }
    }
}
