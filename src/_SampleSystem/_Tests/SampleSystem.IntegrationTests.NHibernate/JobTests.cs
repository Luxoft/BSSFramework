using Framework.Application;
using Framework.AutomationCore.ServiceEnvironment;
using Framework.Database;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.ServiceEnvironment.Jobs;

namespace SampleSystem.IntegrationTests;

public class JobTests : TestBase
{
    [Fact]
    public async Task InvokeJobs_JobObjectsCreated()
    {
        // Arrange
        var prevCount = GetJobInstanceCount();

        var repeatCount = 10;

        // Act
        await Task.WhenAll(Enumerable.Range(0, repeatCount).Select(_ => this.RootServiceProvider.RunJob<SampleJob>()).ToArray());

        // Assert
        var newCount = GetJobInstanceCount();

        (newCount - prevCount).Should().Be(repeatCount);

        int GetJobInstanceCount() =>
            this.Evaluate(
                DBSessionMode.Read,
                c => c.Logics.Default.Create<TestJobObject>().GetUnsecureQueryable().Count());
    }
}
