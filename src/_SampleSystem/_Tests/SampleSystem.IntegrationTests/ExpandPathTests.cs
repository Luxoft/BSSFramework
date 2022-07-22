using System;

using FluentAssertions;

using Framework.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

using Framework.DomainDriven.BLL;

using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class ExpandPathTests : TestBase
    {
        [TestMethod]
        public void LiftToNullableContainsExt_ShouldNotThrowException()
        {
            // Arrange
            var period = this.DateTimeService.CurrentMonth;

            // Act
            var action = new Action(() =>
            {
                var res = this.GetContextEvaluator().Evaluate(DBSessionMode.Read, context => context.Logics.Employee.GetListBy(employee => employee.CoreBusinessUnit.Period.ContainsExt(period.EndDate ?? period.StartDate)));

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
                var res = this.GetContextEvaluator().Evaluate(DBSessionMode.Read, context => context.Logics.Employee.GetListBy(employee => employee.LocationCode == null));

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
                var res = this.GetContextEvaluator().Evaluate(DBSessionMode.Read, context => context.Logics.Employee.GetListBy(employee => employee.CoreBusinessUnitPeriod.IsIntersected(this.DateTimeService.CurrentMonth)));

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
                var res = this.GetContextEvaluator().Evaluate(DBSessionMode.Read, context => context.Logics.Employee.GetListBy(employee => employee.CoreBusinessUnitPeriod.Contains((DateTime?)this.DateTimeService.Today)));

                return;
            });

            // Assert
            action.Should().NotThrow();
        }
    }
}
