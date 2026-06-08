using Anch.Testing.Xunit;

using Framework.Application;
using Framework.Database;

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
                ct), ct);

        // Assert
        await action();
    }
}

