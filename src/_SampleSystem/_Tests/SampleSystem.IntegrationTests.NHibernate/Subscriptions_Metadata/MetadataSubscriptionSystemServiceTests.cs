using System.Text;

using Anch.Testing.Xunit;

using Framework.Core;
using Framework.Notification.Domain;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Models.Custom;
using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests.Subscriptions_Metadata;

public sealed class MetadataSubscriptionSystemServiceTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    protected override async ValueTask InitializeAsync(CancellationToken ct) => this.GetNotifications().Clear();

    [AnchFact]
    public async Task SubscriptionFromMetadataShouldBeSent(CancellationToken ct)
    {
        // Arrange
        var employee = this.CreateEmployee();

        // Act
        var results = await this.DataManager.ProcessSubscriptionAsync(employee, employee, ct);
        var errors = results.GetErrors().ToList();

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "SampleSystem@luxoft.com");

        // Assert
        Assert.Empty(errors);
        var notification = Assert.Single(expectedNotifications);
        Assert.Equal("replayTo@luxoft.com", Assert.Single(notification.Recipients, z => z.Type == RecipientRole.ReplyTo).Name);
    }

    [AnchFact]
    public async Task RazorTemplateImpl_SubscriptionFromMetadataShouldBeSent(CancellationToken ct)
    {
        // Arrange
        var employee = this.DataManager.SaveEmployee("Chuck Norris");
        var message = @"String.Concat it is good choice for Chuck Norris.";

        // Act
        var results = await this.DataManager.ProcessSubscriptionAsync(employee, employee, ct);
        var errors = results.GetErrors().ToList();

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "RazorTemplateImplSubscription@luxoft.com")
                                        .ToList();

        this.LogError(errors);

        // Assert
        Assert.Empty(errors);
        var notification = Assert.Single(expectedNotifications);
        Assert.Equal(message, notification.Message.Message);
        Assert.DoesNotContain(notification.Recipients, z => z.Type == RecipientRole.ReplyTo);
    }

    [AnchFact]
    public async Task LocalRazorTemplate_SubscriptionFromMetadataShouldBeSent(CancellationToken ct)
    {
        // Arrange
        var employee = this.CreateEmployee();
        var message = $"<h2>Hi there!!!</h2>{Environment.NewLine}My test employee Name:  John Doe {Environment.NewLine}Date: 21 Oct 2015";

        // Act
        var results = await this.DataManager.ProcessSubscriptionAsync(employee, employee, ct);
        var errors = results.GetErrors().ToList();

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "RazorInheritanceSubscription@luxoft.com")
                                        .ToList();

        // Assert
        Assert.Empty(errors);
        Assert.Equal(message, Assert.Single(expectedNotifications).Message.Message);
    }

    /// <summary>
    /// IADFRAME-1525 Сделать пример использования аттачей в CodeFirst подписках
    /// </summary>
    /// <remarks>Создать тест: подписка с аттачем, который добавляется в нотификацию</remarks>
    [AnchFact]
    public async Task AttachTest(CancellationToken ct)
    {
        // Arrange
        var employee = this.CreateEmployee();
        var content = Encoding.UTF8.GetBytes("Hello world!");

        // Act
        await this.DataManager.ProcessSubscriptionAsync(employee, employee, ct);

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "Attachment@luxoft.com");

        // Assert
        var notification = expectedNotifications.Single();
        var attachment = notification.Attachments.Single();
        Assert.Equal(content, attachment.Content);
        Assert.Equal(Subscriptions.Metadata.Examples.Attachment.AttachmentSubscription.AttachmentName, attachment.Name);
    }

    /// <summary>
    /// IADFRAME-1525 Сделать пример использования аттачей в CodeFirst подписках
    /// </summary>
    /// <remarks>Создать тест: подписка с аттачем который провернут через шаблонизатор (TemplateEvaluatorFactory) просто текст, который добавляется в нотификацию</remarks>
    [AnchFact]
    public async Task AttachTemplateEvaluatorTest(CancellationToken ct)
    {
        // Arrange
        var employee = this.CreateEmployee();
        var content = "Hello world!  John Doe ";

        // Act
        await this.DataManager.ProcessSubscriptionAsync(employee, employee, ct);

        var expectedNotifications = this.GetNotifications().Where(n => n.From == "AttachmentTemplateEvaluator@luxoft.com");

        // Assert
        var notification = expectedNotifications.Single();
        var attachment = notification.Attachments.Single();
        Assert.Equal(content, Encoding.UTF8.GetString(attachment.Content));
        Assert.Equal(SampleSystem.Subscriptions.Metadata.Examples.AttachmentTemplateEvaluator.AttachmentTemplateEvaluatorSubscription.AttachmentName, attachment.Name);
    }

    /// <summary>
    /// IADFRAME-1525 Сделать пример использования аттачей в CodeFirst подписках
    /// </summary>
    /// <remarks>Создать тест: подписка с inline аттачем (ContentId), который добавляется в нотификацию</remarks>
    [AnchFact]
    public async Task AttachInlinedTest(CancellationToken ct)
    {
        // Arrange
        var employee = this.CreateEmployee();
        var messageTemplate = @"<html><head><title></title></head><body> John Doe <br/><img src=""cid:testId@luxoft.com""/></body></html>";

        // Act
        await this.DataManager.ProcessSubscriptionAsync(employee, employee, ct);

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "InlineAttach@luxoft.com");

        // Assert
        var notification = expectedNotifications.Single();
        var message = string.Format(messageTemplate, notification.Attachments[0].ContentId);
        Assert.Equal(message, notification.Message.Message);
        Assert.Single(notification.Attachments);
    }

    [AnchFact]
    public async Task DateModelCreateSubscriptionTest(CancellationToken ct)
    {
        // Arrange

        // Act
        await this.DataManager.ProcessSubscriptionAsync(null, new DateModel { Year = 2019 }, ct);

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "DateModelCreateSampleSystem@luxoft.com");

        // Assert
        Assert.Single(expectedNotifications);
    }

    private Employee CreateEmployee()
    {
        var employee = this.DataManager.SaveEmployee("John Doe");
        return employee;
    }

    private void LogError(List<Exception> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine(error.Message);
            Console.WriteLine(error.StackTrace);
            Console.WriteLine("-------------------------------------------------------------");
        }
    }
}

