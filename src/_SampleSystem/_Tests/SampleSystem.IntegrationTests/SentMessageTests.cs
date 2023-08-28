using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.Notification.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

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

        var sentMessage = this.EvaluateRead(c => c.Configuration.Logics.SentMessage.GetFullList().Single());

        // Assert
        sentMessage.Copy.Should().Be(expected);
    }
}
