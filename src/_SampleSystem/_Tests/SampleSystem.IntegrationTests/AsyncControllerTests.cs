using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class AsyncControllerTests : TestBase
{
    [TestMethod]
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

        location.Should().NotBeNull();
    }
    [TestMethod]
    public async Task TestSaveLocationWithWriteException_ExceptionRaised()
    {
        // Arrange
        var asyncControllerEvaluator = this.GetControllerEvaluator<TestAsyncController>();

        var saveDto = new LocationStrictDTO { Name = Guid.NewGuid().ToString(), CloseDate = 30, Code = 12345 };

        // Act
        Func<Task> saveTask = () => asyncControllerEvaluator.EvaluateAsync(c => c.AsyncSaveLocationWithWriteException(saveDto, default));

        await saveTask.Should().ThrowAsync<InvalidOperationException>("Invalid session mode. Expected Write");
    }
}
