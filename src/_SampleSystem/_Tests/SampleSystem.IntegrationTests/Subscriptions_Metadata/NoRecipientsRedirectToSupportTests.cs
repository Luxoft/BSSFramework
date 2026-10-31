using Anch.Testing.Xunit;

using Framework.Notification.Domain;

using SampleSystem.Domain.Models.Custom;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.Subscriptions_Metadata;

public abstract class NoRecipientsRedirectToSupportTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    /// <summary>
    /// A subscription without a single recipient (GetTo/GetCopyTo/GetReplyTo/SecurityRoles not set) must
    /// lead to a letter to the support address (RedirectToSupportMailMessageModifier) rather than to the full
    /// to the absence of a notification.
    /// </summary>
    [AnchFact]
    public async Task NoRecipientsSubscription_RedirectsToSupportEmail(CancellationToken ct)
    {
        // Arrange

        // Act
        await this.DataManager.ProcessSubscriptionAsync(null, new NoRecipientModel { Year = 2019 }, ct);

        var notifications = this.GetNotifications().Where(n => n.From == "NoRecipients@luxoft.com").ToList();

        // Assert
        var notification = Assert.Single(notifications);

        Assert.Equal(
            "SampleSystemSupport@luxoft.com",
            Assert.Single(notification.Recipients, r => r.Type == RecipientRole.To).Name);
    }
}
