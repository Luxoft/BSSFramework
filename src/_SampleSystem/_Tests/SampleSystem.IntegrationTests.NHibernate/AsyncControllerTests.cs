using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class AsyncControllerTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public async Task TestSaveLocation_LocationSaved()
    {
        // Arrange
        var asyncControllerEvaluator = this.GetControllerEvaluator<TestAsyncController>();

        var saveDto = new LocationStrictDTO { Name = Guid.NewGuid().ToString(), CloseDate = 30, Code = 12345 };

        // Act
        var ident = await asyncControllerEvaluator.EvaluateAsync(c => c.AsyncSaveLocation(saveDto, default));

        var loadedLocationList = await asyncControllerEvaluator.EvaluateAsync(c => c.AsyncGetLocations(default));

        // Assert
        var location = loadedLocationList.SingleOrDefault(bu => bu.Name == saveDto.Name && bu.Identity == ident);

        Assert.NotNull(location);
    }

    [Fact]
    public async Task TestSaveLocationWithWriteException_ExceptionRaised()
    {
        // Arrange
        var asyncControllerEvaluator = this.GetControllerEvaluator<TestAsyncController>();

        var saveDto = new LocationStrictDTO { Name = Guid.NewGuid().ToString(), CloseDate = 30, Code = 12345 };

        // Act
        var ex = await Record.ExceptionAsync(() => asyncControllerEvaluator.EvaluateAsync(c => c.AsyncSaveLocationWithWriteException(saveDto, default)));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
    }
}
