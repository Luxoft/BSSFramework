using Anch.Testing.Xunit;

using Framework.Notification.Domain;
using Framework.Notification.DTO;

using SampleSystem.Domain.Models.Custom;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.Subscriptions.Metadata.Examples.IndividualLetters;

namespace SampleSystem.IntegrationTests.Subscriptions_Metadata;

public abstract class SendIndividualLettersTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    /// <summary>
    /// A subscription with SendIndividualLetters == true forms a separate letter for each delivery addressee
    /// </summary>
    [AnchFact]
    public async Task SendIndividualLetters_IndividualNotificationSentToEachRecipient(CancellationToken ct)
    {
        // Arrange

        // Act
        await this.DataManager.ProcessSubscriptionAsync(null, new DateModel { Year = 2019 }, ct);

        var notifications = this.GetIndividualLettersNotifications("IndividualLetters@luxoft.com");

        // Assert
        Assert.Equal(3, notifications.Count);

        Assert.All(notifications, n => Assert.Single(n.Recipients, r => r.Type != RecipientRole.ReplyTo));

        Assert.Equal(
            [IndividualLettersSubscriptionBase.FirstToRecipient, IndividualLettersSubscriptionBase.SecondToRecipient],
            [.. notifications.SelectMany(n => n.Recipients).Where(r => r.Type == RecipientRole.To).Select(r => r.Name).Order()]);

        Assert.Equal(
            IndividualLettersSubscriptionBase.CopyRecipient,
            Assert.Single(notifications.SelectMany(n => n.Recipients), r => r.Type == RecipientRole.Copy).Name);

        // ReplyTo is not a delivery addressee, so it goes into every letter
        Assert.All(
            notifications,
            n => Assert.Equal(
                IndividualLettersSubscriptionBase.ReplyToRecipient,
                Assert.Single(n.Recipients, r => r.Type == RecipientRole.ReplyTo).Name));

        Assert.All(notifications, n => Assert.Contains("2019", n.Message.Message));
    }

    /// <summary>
    /// A subscription with SendIndividualLetters == false forms one consolidated letter for all addressees
    /// </summary>
    [AnchFact]
    public async Task SendIndividualLetters_Disabled_ConsolidatedNotificationSent(CancellationToken ct)
    {
        // Arrange

        // Act
        await this.DataManager.ProcessSubscriptionAsync(null, new DateModel { Year = 2019 }, ct);

        var notifications = this.GetIndividualLettersNotifications("ConsolidatedLetter@luxoft.com");

        // Assert
        var notification = Assert.Single(notifications);

        Assert.Equal(
            [IndividualLettersSubscriptionBase.FirstToRecipient, IndividualLettersSubscriptionBase.SecondToRecipient],
            [.. notification.Recipients.Where(r => r.Type == RecipientRole.To).Select(r => r.Name).Order()]);

        Assert.Equal(
            IndividualLettersSubscriptionBase.CopyRecipient,
            Assert.Single(notification.Recipients, r => r.Type == RecipientRole.Copy).Name);

        Assert.Equal(
            IndividualLettersSubscriptionBase.ReplyToRecipient,
            Assert.Single(notification.Recipients, r => r.Type == RecipientRole.ReplyTo).Name);
    }

    /// <summary>
    /// Individual letters are formed for each version of the domain object independently
    /// </summary>
    [AnchFact]
    public async Task SendIndividualLetters_SeveralDomainObjects_IndividualNotificationsSentForEach(CancellationToken ct)
    {
        // Arrange

        // Act
        await this.DataManager.ProcessSubscriptionAsync(null, new DateModel { Year = 2019 }, ct);
        await this.DataManager.ProcessSubscriptionAsync(null, new DateModel { Year = 2020 }, ct);

        var notifications = this.GetIndividualLettersNotifications("IndividualLetters@luxoft.com");

        // Assert
        Assert.Equal(6, notifications.Count);

        Assert.Equal(3, notifications.Count(n => n.Message.Message.Contains("2019")));
        Assert.Equal(3, notifications.Count(n => n.Message.Message.Contains("2020")));

        Assert.All(notifications, n => Assert.Single(n.Recipients, r => r.Type != RecipientRole.ReplyTo));
    }

    private List<NotificationEventDTO> GetIndividualLettersNotifications(string from) =>
        [.. this.GetNotifications().Where(n => n.From == from)];
}
