using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class NhibDateTimeTests : TestBase
    {
        private DateTime? prevDateTime;

        [TestInitialize]
        public void SetUp()
        {
            this.prevDateTime = IntegrationTestDateTimeService.CurrentDate;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            IntegrationTestDateTimeService.CurrentDate = this.prevDateTime;
        }

        [TestMethod]
        public void CreateObject_CreatedDateOverride()
        {
            // Arrange
            var testDate = new DateTime(2000, 5, 5);
            IntegrationTestDateTimeService.CurrentDate = testDate;

            var example1Controller = this.GetControllerEvaluator<Example1Controller>();

            // Act
            var objIdentity = example1Controller.Evaluate(c => c.SaveExample1(new Example1StrictDTO()));

            // Assert
            var reloadedObj = example1Controller.Evaluate(c => c.GetSimpleExample1(objIdentity));

            reloadedObj.CreateDate.Should().Be(testDate);
        }
    }
}
