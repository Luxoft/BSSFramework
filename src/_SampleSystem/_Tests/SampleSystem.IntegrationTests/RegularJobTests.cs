using System;
using System.Linq;

using FluentAssertions;

using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class RegularJobTests : TestBase
    {
        /// <summary>
        /// Тест игнорирования job-а, если он не находится в WaitPulse статусе
        /// </summary>
        [TestMethod]
        public void GetPulseJobs_IgnoreJob()
        {
            // Arrange
            this.ClearRegularJobs();

            var jobIdentity = this.GetConfigurationController().SaveRegularJob(new RegularJobStrictDTO
            {
                Name = "TestJob" + Guid.NewGuid(),
                ShedulerTime = ShedulerTime.FromDays(1),
                ExpectedNextStartTime = DateTime.Now,
                WrapUpSession = true,
                Function = "context => {}"
            });

            var res = this.GetConfigurationController().GetPulseJobs(); // First Invoke

            // Act
            var expectedJobs = this.GetConfigurationController().GetPulseJobs();

            // Assert
            expectedJobs.Should().NotContain(dto => dto.RegularJob == jobIdentity);
        }

        /// <summary>
        /// Тест переключения статуса job-а в SendToProcessing при вызове метода GetPulseJobs
        /// </summary>
        [TestMethod]
        public void GetPulseJobs_HasProcessingState()
        {
            // Arrange
            this.ClearRegularJobs();

            var jobIdentity = this.GetConfigurationController().SaveRegularJob(new RegularJobStrictDTO
            {
                Name = "TestJob" + Guid.NewGuid(),
                ShedulerTime = ShedulerTime.FromDays(1),
                ExpectedNextStartTime = DateTime.Now,
                WrapUpSession = true,
                Function = "context => {}"
            });

            // Act
            var expectedJobs = this.GetConfigurationController().GetPulseJobs();

            // Assert
            var postJobState = this.GetConfigurationController().GetRichRegularJob(jobIdentity);

            expectedJobs.Should().Contain(dto => dto.RegularJob == jobIdentity);
            postJobState.State.Should().Be(RegularJobState.SendToProcessing);
        }

        /// <summary>
        /// Тест отработки job-а при вызове метода SyncPulseJobs
        /// </summary>
        [TestMethod]
        public void SyncPulseJobs_ExistsResult()
        {
            // Arrange
            var regularJobResultController = this.GetController<RegularJobResultController>();
            this.ClearRegularJobs();

            var testValue = Guid.NewGuid().ToString();

            var jobIdentity = this.GetConfigurationController().SaveRegularJob(new RegularJobStrictDTO
            {
                Name = "TestJob" + Guid.NewGuid(),
                ShedulerTime = ShedulerTime.FromDays(1),
                ExpectedNextStartTime = DateTime.Now,
                WrapUpSession = false,
                Function = $"environment => environment.SaveRegularJobTestValue(\"{testValue}\")"
            });

            // Act
            this.GetConfigurationController().SyncPulseJobs();

            // Assert
            var postJobState = this.GetConfigurationController().GetRichRegularJob(jobIdentity);

            postJobState.State.Should().Be(RegularJobState.WaitPulse);

            regularJobResultController.GetSimpleRegularJobResults().Should().Contain(dto => dto.TestValue == testValue);
        }

        /// <summary>
        /// Тест переключения статуса job-а в SendToProcessing при вызове метода PulseJobs (MSMQ)
        /// </summary>
        [TestMethod]
        public void AsyncPulseJobs_HasProcessingState()
        {
            // Arrange
            this.ClearRegularJobs();

            var jobIdentity = this.GetConfigurationController().SaveRegularJob(new RegularJobStrictDTO
            {
                Name = "TestJob" + Guid.NewGuid(),
                ShedulerTime = ShedulerTime.FromDays(1),
                ExpectedNextStartTime = DateTime.Now,
                WrapUpSession = true,
                Function = "context => {}"
            });

            // Act
            this.GetConfigurationController().PulseJobs();

            // Assert
            var postJobState = this.GetConfigurationController().GetRichRegularJob(jobIdentity);

            postJobState.State.Should().Be(RegularJobState.SendToProcessing);

            this.GetRegularJobsMessages().Should().Contain(dto => dto.RegularJob == jobIdentity);
        }

        /// <summary>
        /// Тест отработки job-а при вызове метода RunRegularJob (MSMQ)
        /// </summary>
        [TestMethod]
        public void AsyncPulseJobs_ExistsResult()
        {
            // Arrange
            this.ClearRegularJobs();

            var testValue = Guid.NewGuid().ToString();

            var jobIdentity = this.GetConfigurationController().SaveRegularJob(new RegularJobStrictDTO
            {
                Name = "TestJob" + Guid.NewGuid(),
                ShedulerTime = ShedulerTime.FromDays(1),
                ExpectedNextStartTime = DateTime.Now,
                WrapUpSession = true,
                Function = $"context => context.Logics.RegularJobResult.SaveTestValue(\"{testValue}\")"
            });

            this.GetConfigurationController().PulseJobs();

            var message = this.GetRegularJobsMessages().Single(dto => dto.RegularJob == jobIdentity);

            // Act
            this.GetConfigurationController().RunRegularJob(message);

            // Assert
            var postJobState = this.GetConfigurationController().GetRichRegularJob(jobIdentity);

            postJobState.State.Should().Be(RegularJobState.WaitPulse);

            this.GetRegularJobsMessages().Should().Contain(dto => dto.RegularJob == jobIdentity);
        }

        /// <summary>
        /// Тест игнорирования job-а, если признак Active выключен
        /// </summary>
        [TestMethod]
        public void GetPulseDisabledJobs_IgnoreJob()
        {
            // Arrange
            this.ClearRegularJobs();

            var jobIdentity = this.GetConfigurationController().SaveRegularJob(new RegularJobStrictDTO
            {
                Active = false,
                Name = "TestJob" + Guid.NewGuid(),
                ShedulerTime = ShedulerTime.FromDays(1),
                ExpectedNextStartTime = DateTime.Now,
                WrapUpSession = true,
                Function = "context => {}"
            });

            // Act
            var expectedJobs = this.GetConfigurationController().GetPulseJobs();

            // Assert
            expectedJobs.Should().NotContain(dto => dto.RegularJob == jobIdentity);
        }
    }
}
