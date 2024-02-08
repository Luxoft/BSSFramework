using Automation.ServiceEnvironment;

using DotNetCore.CAP;

using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain.TestForceAbstract;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers;

namespace SampleSystem.IntegrationTests.Transactions;

[TestClass]
public class TransactionsTest : TestBase
{
    [TestMethod]
    public async Task RepositoryNoSession_TransactionShouldNotBeCompletedInTheMiddleOfSession()
    {
        // Arrange
        await this.RootServiceProvider.GetService<IBootstrapper>().BootstrapAsync();

        // Act
        await this.GetControllerEvaluator<ClassAAsyncController>()
                  .EvaluateAsync(c => c.CreateClassA(1234, false, default));
        await Task.Delay(5000, default);

        // Assert
        this.EvaluateRead(ctx => ctx.Logics.Default.Create<ClassA>().GetUnsecureQueryable().Should().BeEmpty());
    }

    [TestMethod]
    public async Task RepositoryWithSession_TransactionShouldNotBeCompletedInTheMiddleOfSession()
    {
        // Arrange
        await this.RootServiceProvider.GetService<IBootstrapper>().BootstrapAsync();

        // Act
        await this.GetControllerEvaluator<ClassAAsyncController>()
                  .EvaluateAsync(c => c.CreateClassA(4321, true, default));
        await Task.Delay(5000, default);

        // Assert
        this.EvaluateRead(ctx => ctx.Logics.Default.Create<ClassA>().GetUnsecureQueryable().Should().BeEmpty());
    }
}
