using Anch.Testing.Xunit;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class AsyncControllerTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task TestSaveLocation_LocationSaved(CancellationToken ct)
    {
        // Arrange
        var asyncControllerEvaluator = this.GetControllerEvaluator<TestAsyncController>();

        var saveDto = new LocationStrictDTO { Name = Guid.NewGuid().ToString(), CloseDate = 30, Code = 12345 };

        // Act
        var ident = await asyncControllerEvaluator.EvaluateAsync(c => c.AsyncSaveLocation(saveDto, ct));

        var loadedLocationList = await asyncControllerEvaluator.EvaluateAsync(c => c.AsyncGetLocations(ct));

        // Assert
        var location = loadedLocationList.SingleOrDefault(bu => bu.Name == saveDto.Name && bu.Identity == ident);

        Assert.NotNull(location);
    }

    [AnchFact]
    public async Task TestSaveLocationWithWriteException_ExceptionRaised(CancellationToken ct)
    {
        // Arrange
        var asyncControllerEvaluator = this.GetControllerEvaluator<TestAsyncController>();

        var saveDto = new LocationStrictDTO { Name = Guid.NewGuid().ToString(), CloseDate = 30, Code = 12345 };

        // Act
        var ex = await Record.ExceptionAsync(() => asyncControllerEvaluator.EvaluateAsync(c => c.AsyncSaveLocationWithWriteException(saveDto, ct)));

        // Assert
        Assert.IsType<InvalidOperationException>(ex);
    }
}
