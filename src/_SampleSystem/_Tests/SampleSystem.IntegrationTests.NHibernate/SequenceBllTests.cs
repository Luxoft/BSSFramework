using Framework.Application;
using Framework.Configuration.BLL;
using Framework.Database;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class SequenceBllTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void GetNextNumber_TwoCallsInParallelTransactions_ShouldGiveDifferentNumbers()
    {
        // Arrange
        const string name = "Test";
        this.Evaluate(DBSessionMode.Write, ctx => ctx.Configuration.Logics.Sequence.GetNextNumber(name));

        var numberWithWait1 = 0L;
        var numberWithWait2 = 0L;
        var numberFast1 = 0L;
        var numberFast2 = 0L;

        var autoResetEvent = new AutoResetEvent(false);
        var autoResetEvent2 = new AutoResetEvent(false);

        // Act
        var taskFast = Task.Factory.StartNew(
            () =>
            {
                this.Evaluate(
                    DBSessionMode.Write,
                    ctx =>
                    {
                        var sequenceBllMock = new SequenceBllMock(autoResetEvent, ctx.Configuration);
                        numberFast1 = sequenceBllMock.GetNextNumber(name);
                        numberFast2 = sequenceBllMock.GetNextNumber(name);

                        autoResetEvent2.WaitOne();
                    });
            });

        var taskWithWait = Task.Factory.StartNew(
            () =>
            {
                this.Evaluate(
                    DBSessionMode.Write,
                    ctx =>
                    {
                        autoResetEvent.WaitOne();

                        var sequenceBllMock = new SequenceBllMock(autoResetEvent2, ctx.Configuration);
                        numberWithWait1 = sequenceBllMock.GetNextNumber(name);
                        numberWithWait2 = sequenceBllMock.GetNextNumber(name);
                    });
            });

        // Assert
        Task.WaitAll(taskWithWait, taskFast);
        var numbers = new[] { numberFast1, numberFast2, numberWithWait1, numberWithWait2 };
        Assert.Equal(numbers.Length, numbers.Distinct().Count());
    }

    [Fact]
    public void GetNextNumber_TwoCallsOneTransaction_ShouldGiveDifferentNumbers()
    {
        // Arrange
        const string name = "Test";
        this.Evaluate(DBSessionMode.Write, ctx => ctx.Configuration.Logics.Sequence.GetNextNumber(name));

        // Act
        var numbers = this.Evaluate(
            DBSessionMode.Write,
            context =>
            {
                var number1 = context.Configuration.Logics.Sequence.GetNextNumber(name);
                var number2 = context.Configuration.Logics.Sequence.GetNextNumber(name);
                return (Number1: number1, Number2: number2);
            });

        // Assert
        Assert.Equal(2, numbers.Number1);
        Assert.Equal(3, numbers.Number2);
    }

    [Fact]
    public void GetNextNumber_TwoCallsDifferentTransaction_ShouldGiveDifferentNumbers()
    {
        // Arrange
        const string name = "Test";
        this.Evaluate(DBSessionMode.Write, ctx => ctx.Configuration.Logics.Sequence.GetNextNumber(name));

        var number1 = this.Evaluate(
            DBSessionMode.Write,
            context => context.Configuration.Logics.Sequence.GetNextNumber(name));

        // Act
        var number2 = this.Evaluate(
            DBSessionMode.Write,
            context => context.Configuration.Logics.Sequence.GetNextNumber(name));

        // Assert
        Assert.Equal(2, number1);
        Assert.Equal(3, number2);
    }

    private class SequenceBllMock(
        AutoResetEvent resetEvent,
        IConfigurationBLLContext context)
        : SequenceBLL(context, context.Logics.Sequence.SecurityProvider)
    {
        protected override void LockSequence()
        {
            resetEvent.Set();
            base.LockSequence();
        }
    }
}
