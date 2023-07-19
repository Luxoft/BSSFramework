using FluentAssertions;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using nuSpec.Abstraction;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class SequenceBllTests : TestBase
{
    [DataTestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void GetEmployeeFromDB_FilterByAge_ReturnNotNulRecords(bool hasRefresh)
    {
        // Arrange
        const string name = "Test";
        this.Evaluate(DBSessionMode.Write, ctx => ctx.Configuration.Logics.Sequence.GetNextNumber(name));

        var task1Number = 0L;
        var task2Number = 0L;

        var autoResetEvent = new AutoResetEvent(false);

        // Act
        var taskFirst = Task.Factory.StartNew(
            () =>
            {
                task1Number = this.Evaluate(
                    DBSessionMode.Write,
                    ctx =>
                    {
                        autoResetEvent.WaitOne();
                        return new SequenceBllMock(hasRefresh, ctx.Configuration, ctx.Configuration.Logics.Sequence.SecurityProvider)
                            .GetNextNumber(name);
                    });
            });

        var taskSecond = Task.Factory.StartNew(
            () =>
            {
                task2Number = this.Evaluate(
                    DBSessionMode.Write,
                    ctx =>
                    {
                        var nextNumber = new SequenceBllMock(
                                hasRefresh,
                                ctx.Configuration,
                                ctx.Configuration.Logics.Sequence.SecurityProvider)
                            .GetNextNumber(name);

                        autoResetEvent.Set();
                        Thread.Sleep(100);
                        return nextNumber;
                    });
            });

        // Assert
        Task.WaitAll(taskFirst, taskSecond);
        (task1Number != task2Number).Should().Be(hasRefresh);
    }

    private class SequenceBllMock : SequenceBLL
    {
        private readonly bool hasRefresh;

        public SequenceBllMock(
            bool hasRefresh,
            IConfigurationBLLContext context,
            ISecurityProvider<Sequence> securityProvider,
            ISpecificationEvaluator specificationEvaluator = null)
            : base(context, securityProvider, specificationEvaluator) =>
            this.hasRefresh = hasRefresh;

        public override void Refresh(Sequence domainObject)
        {
            if (this.hasRefresh)
            {
                base.Refresh(domainObject);
            }
        }
    }
}
