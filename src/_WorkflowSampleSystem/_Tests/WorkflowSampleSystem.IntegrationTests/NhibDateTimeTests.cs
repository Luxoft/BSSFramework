using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.WebApiCore.Controllers.Main;

namespace WorkflowSampleSystem.IntegrationTests
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

            var example1Controller = this.GetController<Example1Controller>();

            // Act
            var objIdentity = example1Controller.SaveExample1(new Example1StrictDTO());

            // Assert
            var reloadedObj = example1Controller.GetSimpleExample1(objIdentity);

            reloadedObj.CreateDate.Should().Be(testDate);
        }
    }
}
