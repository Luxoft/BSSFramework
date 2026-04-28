using Framework.Application;
using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.Core;
using Framework.Database;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class ExpandPathTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
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
        action();
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
        action();
    }

    [Fact]
    public void LiftToNullablePeriodIntersect_ShouldNotThrowException()
    {
        // Arrange

        // Act
        var action = new Action(() =>
                                {
                                    var currentMonth = this.TimeProvider.GetCurrentMonth();

                                    var res = this.Evaluate(
                                        DBSessionMode.Read,
                                        context => context.Logics.Employee.GetListBy(
                                            employee => employee.CoreBusinessUnitPeriod.IsIntersected(currentMonth)));

                                    return;
                                });

        // Assert
        action();
    }


    [Fact]
    public void LiftToNullablePeriodContains_ShouldNotThrowException()
    {
        // Arrange

        // Act
        var action = new Action(() =>
        {
            var today = this.TimeProvider.GetToday();

            var res = this.Evaluate(
                DBSessionMode.Read,
                context => context.Logics.Employee.GetListBy(employee => employee.CoreBusinessUnitPeriod.Contains(today)));

            return;
        });

        // Assert
        action();
    }
}
