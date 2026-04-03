using Framework.Application;
using Framework.Database;

using SampleSystem.EventMetadata;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class EventDALListenerTests : TestBase
{
    [TestMethod]
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
