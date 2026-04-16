using Framework.Application;
using Framework.Core;
using Framework.Database;

using SampleSystem.IntegrationTests.__Support.TestData;

using Xunit;

namespace SampleSystem.IntegrationTests;

public class ExpandPathTests : TestBase
{
    [Fact]
    public void LiftToNullableContainsExt_ShouldNotThrowException()
    {
        // Arrange
        var period = this.TimeProvider.GetCurrentMonth();

        // Act
        var action = new Action(() =>
                                {
                                    var res = this.Evaluate(DBSessionMode.Read, context => context.Logics.Employee.GetListBy(employee => employee.CoreBusinessUnit.Period.ContainsExt(period.EndDate ?? period.StartDate)));

                                    return;
                                });

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void LiftToNullableBinaryExpression_ShouldNotThrowException()
    {
        // Arrange

        // Act
        var action = new Action(() =>
                                {
                                    var res = this.Evaluate(DBSessionMode.Read, context => context.Logics.Employee.GetListBy(employee => employee.LocationCode == null));

                                    return;
                                });

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void LiftToNullablePeriodIntersect_ShouldNotThrowException()
    {
        // Arrange

        // Act
        var action = new Action(() =>
                                {
                                    var res = this.Evaluate(
                                        DBSessionMode.Read,
                                        context => context.Logics.Employee.GetListBy(
                                            employee => employee.CoreBusinessUnitPeriod.IsIntersected(
                                                this.TimeProvider.GetCurrentMonth())));

                                    return;
                                });

        // Assert
        action.Should().NotThrow();
    }


    [Fact]
    public void LiftToNullablePeriodContains_ShouldNotThrowException()
    {
        // Arrange

        // Act
        var action = new Action(() =>
                                {
                                    var res = this.Evaluate(DBSessionMode.Read, context => context.Logics.Employee.GetListBy(employee => employee.CoreBusinessUnitPeriod.Contains(this.TimeProvider.GetToday())));

                                    return;
                                });

        // Assert
        action.Should().NotThrow();
    }
}
