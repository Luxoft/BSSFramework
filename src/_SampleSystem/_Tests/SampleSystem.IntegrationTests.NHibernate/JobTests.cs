using Anch.Testing.Xunit;

using Framework.Application;
using Framework.AutomationCore.Extensions;
using Framework.Database;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.ServiceEnvironment.Jobs;

namespace SampleSystem.IntegrationTests;

public class JobTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [AnchFact]
    public async Task InvokeJobs_JobObjectsCreated(CancellationToken ct)
    {
        // Arrange
        var prevCount = GetJobInstanceCount();

        var repeatCount = 10;

        // Act
        await Task.WhenAll(Enumerable.Range(0, repeatCount).Select(_ => this.RootServiceProvider.RunJob<SampleJob>()).ToArray());

        // Assert
        var newCount = GetJobInstanceCount();

        Assert.Equal(repeatCount, newCount - prevCount);

        int GetJobInstanceCount() =>
            this.Evaluate(
                DBSessionMode.Read,
                c => c.Logics.Default.Create<TestJobObject>().GetUnsecureQueryable().Count());
    }
}

