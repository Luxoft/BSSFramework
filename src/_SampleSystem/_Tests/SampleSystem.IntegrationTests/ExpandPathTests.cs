using FluentAssertions;

using Framework.Core;
using Framework.DomainDriven;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class ExpandPathTests : TestBase
{
    [TestMethod]
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

    [TestMethod]
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

    [TestMethod]
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


    [TestMethod]
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
