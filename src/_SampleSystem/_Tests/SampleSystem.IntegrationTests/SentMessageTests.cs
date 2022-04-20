using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Notification.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class SentMessageTests : TestBase
    {
        [TestMethod]
        public void Save_LongTextInCopy_SavesWithoutErrorOrTruncate()
        {
            // Arrange
            var copy = "copyText10";
            var targets = Enumerable.Range(1, 30).Select(x => new NotificationTargetDTO { Name = copy, Type = NotificationTargetTypes.Copy }).ToList();

            var notification = new NotificationEventDTO
            {
                Message = new NotificationMessage(),
                TechnicalInformation = new NotificationTechnicalInformationDTO(),
                Targets = targets
            };

            var expected = string.Join(",", targets.Select(x => x.Name));

            // Act
            this.GetConfigurationControllerEvaluator().Evaluate(c => c.SaveSendedNotification(notification));
            var sentMessage = this.GetConfigurationControllerEvaluator().Evaluate(c => c.EvaluateRead(evaluatedData => new SentMessageBLL(evaluatedData.Context.Configuration).GetFullList().Single()));

            // Assert
            sentMessage.Copy.Should().Be(expected);
        }
    }
}
