using Framework.Application;
using Framework.Database;

using Anch.Testing.Xunit;

using SampleSystem.EventMetadata;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class EventDALListenerTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task Employee_SendCustomEventOperation_ExceptionNotThrow(CancellationToken ct)
    {
        //Arrange

        // Act
        var action = () => this.EvaluateAsync(
            DBSessionMode.Write,
            ctx => ctx.OperationSender.Send(
                ctx.CurrentEmployeeSource.CurrentUser,
                SampleSystemEventOperation.CustomAction,
                ct));

        // Assert
        await action();
    }
}
