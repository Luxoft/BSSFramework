using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.DomainDriven;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.BLL.Jobs;
using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

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
