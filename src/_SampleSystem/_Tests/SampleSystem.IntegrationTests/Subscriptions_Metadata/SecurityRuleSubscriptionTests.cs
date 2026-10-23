using Anch.Testing.Xunit;

using Framework.Notification.Domain;

using SampleSystem.Domain.Models.Custom;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.Security;
using SampleSystem.Subscriptions.Metadata.Examples.SecurityRule;

namespace SampleSystem.IntegrationTests.Subscriptions_Metadata;

public abstract class SecurityRuleSubscriptionTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    private const string SecurityRuleSender = "SecurityRule@luxoft.com";

    private const string MatchedLogin = nameof(MatchedLogin);

    private const string MissedLogin = nameof(MissedLogin);

    private const string MatchedEmail = "securityRuleMatched@luxoft.com";

    private const string MissedEmail = "securityRuleMissed@luxoft.com";

    /// <summary>
    /// Получателями подписки с SecurityRule становятся сотрудники, чьи принципалы имеют роль на контекст из GetNotificationFilterGroups
    /// </summary>
    [AnchFact]
    public async Task SecurityRuleSubscription_PrincipalWithRoleOnFilterContext_ReceivesNotification(CancellationToken ct)
    {
        // Arrange
        var businessUnit = this.DataManager.SaveBusinessUnit(id: SecurityRuleSubscription.BusinessUnitId);
        var otherBusinessUnit = this.DataManager.SaveBusinessUnit();

        this.DataManager.SaveEmployee(login: MatchedLogin, email: MatchedEmail);
        this.DataManager.SaveEmployee(login: MissedLogin, email: MissedEmail);

        await this.AuthManager.For(MatchedLogin).SetRoleAsync(
            new SampleSystemTestPermission(SampleSystemSecurityRole.SearchTestBusinessRole, businessUnit),
            ct);

        await this.AuthManager.For(MissedLogin).SetRoleAsync(
            new SampleSystemTestPermission(SampleSystemSecurityRole.SearchTestBusinessRole, otherBusinessUnit),
            ct);

        // Act
        await this.DataManager.ProcessSubscriptionAsync(null, new NoRecipientModel { Year = 2019 }, ct);

        var notification = Assert.Single(this.GetNotifications(), n => n.From == SecurityRuleSender);

        // Assert
        Assert.Equal(MatchedEmail, Assert.Single(notification.Recipients, r => r.Type == RecipientRole.To).Name);
        Assert.Contains("2019", notification.Message.Message);
    }

    /// <summary>
    /// Принципал с ролью на другой контекст (не из GetNotificationFilterGroups) не получает уведомление
    /// </summary>
    [AnchFact]
    public async Task SecurityRuleSubscription_PrincipalWithRoleOnOtherContext_NotReceivesNotification(CancellationToken ct)
    {
        // Arrange
        this.DataManager.SaveBusinessUnit(id: SecurityRuleSubscription.BusinessUnitId);
        var otherBusinessUnit = this.DataManager.SaveBusinessUnit();

        this.DataManager.SaveEmployee(login: MissedLogin, email: MissedEmail);

        await this.AuthManager.For(MissedLogin).SetRoleAsync(
            new SampleSystemTestPermission(SampleSystemSecurityRole.SearchTestBusinessRole, otherBusinessUnit),
            ct);

        // Act
        await this.DataManager.ProcessSubscriptionAsync(null, new NoRecipientModel { Year = 2019 }, ct);

        var notifications = this.GetNotifications().Where(n => n.From == SecurityRuleSender);

        // Assert
        Assert.DoesNotContain(
            notifications.SelectMany(n => n.Recipients),
            r => r.Name == MissedEmail);
    }
}
