using Anch.Testing.Xunit;

using Framework.Core;
using Framework.Notification.Domain;

using SampleSystem.Domain.Models.Custom;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests.Subscriptions_Metadata;

/// <summary>
/// Subscription with SecurityRule but without filter groups: every employee with the role on any context is a recipient, and with nobody left the letter goes to support.
/// </summary>
public abstract class SecurityRuleNoFilterTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    private const string Sender = "SecurityRuleNoFilter@luxoft.com";

    private const string SupportEmail = "SampleSystemSupport@luxoft.com";

    private const string BadLogin = nameof(BadLogin);

    private const string ValidEmail = "securityRuleNoFilterValid@luxoft.com";

    /// <summary>
    /// Control: an employee with a valid email receives the notification
    /// </summary>
    [AnchFact]
    public async Task SecurityRuleNoFilter_EmployeeWithValidEmail_ReceivesNotification(CancellationToken ct)
    {
        // Arrange
        await this.PrepareEmployeeWithRoleAsync(ValidEmail, ct);

        // Act
        var results = await this.DataManager.ProcessSubscriptionAsync(null, new NoRecipientModel { Year = 2019 }, ct);

        // Assert
        Assert.Empty(results.GetErrors());

        var notification = Assert.Single(this.GetNotifications(), n => n.From == Sender);

        Assert.Contains(notification.Recipients, r => r.Type == RecipientRole.To && r.Name == ValidEmail);
    }

    /// <summary>
    /// Without filter groups the notification is sent unconditionally to every employee that has the role (on any context),
    /// and is not sent to employees without the role
    /// </summary>
    [AnchFact]
    public async Task SecurityRuleNoFilter_SeveralEmployees_AllRoleHoldersReceiveNotification(CancellationToken ct)
    {
        // Arrange
        const string secondLogin = "SecondRoleHolder";
        const string secondEmail = "securityRuleNoFilterSecond@luxoft.com";
        const string noRoleLogin = "NoRoleEmployee";
        const string noRoleEmail = "securityRuleNoFilterNoRole@luxoft.com";

        var businessUnit = this.DataManager.SaveBusinessUnit();
        var anotherBusinessUnit = this.DataManager.SaveBusinessUnit();

        await this.PrepareEmployeeWithRoleAsync(ValidEmail, ct);

        this.DataManager.SaveEmployee(login: secondLogin, email: secondEmail);

        await this.AuthManager.For(secondLogin).SetRoleAsync(
            new SampleSystemTestPermission(SampleSystemSecurityRole.SearchTestBusinessRole, anotherBusinessUnit),
            ct);

        this.DataManager.SaveEmployee(login: noRoleLogin, email: noRoleEmail);

        // Act
        var results = await this.DataManager.ProcessSubscriptionAsync(null, new NoRecipientModel { Year = 2019 }, ct);

        // Assert
        Assert.Empty(results.GetErrors());

        var notification = Assert.Single(this.GetNotifications(), n => n.From == Sender);
        var to = notification.Recipients.Where(r => r.Type == RecipientRole.To).Select(r => r.Name).ToList();

        Assert.Contains(ValidEmail, to);
        Assert.Contains(secondEmail, to);
        Assert.DoesNotContain(noRoleEmail, to);
        Assert.DoesNotContain(SupportEmail, to);
    }

    /// <summary>
    /// Nobody has the role: the list of recipients is empty and the letter goes to the support address
    /// </summary>
    [AnchFact]
    public async Task SecurityRuleNoFilter_NobodyHasRole_NotificationGoesToSupport(CancellationToken ct)
    {
        // Arrange
        this.DataManager.SaveEmployee(login: BadLogin, email: ValidEmail);

        // Act
        var results = await this.DataManager.ProcessSubscriptionAsync(null, new NoRecipientModel { Year = 2019 }, ct);

        // Assert
        Assert.Empty(results.GetErrors());

        var notification = Assert.Single(this.GetNotifications(), n => n.From == Sender);

        Assert.Equal(SupportEmail, Assert.Single(notification.Recipients, r => r.Type == RecipientRole.To).Name);
    }

    private async Task PrepareEmployeeWithRoleAsync(string email, CancellationToken ct)
    {
        var businessUnit = this.DataManager.SaveBusinessUnit();

        this.DataManager.SaveEmployee(login: BadLogin, email: email);

        await this.AuthManager.For(BadLogin).SetRoleAsync(
            new SampleSystemTestPermission(SampleSystemSecurityRole.SearchTestBusinessRole, businessUnit),
            ct);
    }
}
