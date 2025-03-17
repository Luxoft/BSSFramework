using Automation.ServiceEnvironment;

using Framework.DomainDriven;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.ServiceEnvironment.Jobs;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class JobTests : TestBase
{
    [TestMethod]
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
