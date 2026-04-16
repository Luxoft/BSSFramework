using Framework.Application;
using Framework.Database;

using SampleSystem.EventMetadata;
using SampleSystem.IntegrationTests.__Support.TestData;

using Xunit;

namespace SampleSystem.IntegrationTests;

public class EventDALListenerTests : TestBase
{
    [Fact]
    public async Task Employee_SendCustomEventOperation_ExceptionNotThrow()
    {
        //Arrange

        // Act
        var action = () => this.EvaluateAsync(
            DBSessionMode.Write,
            ctx => ctx.OperationSender.Send(
                ctx.CurrentEmployeeSource.CurrentUser,
                SampleSystemEventOperation.CustomAction,
                CancellationToken.None));

        // Assert
        await action.Should().NotThrowAsync();
    }
}
