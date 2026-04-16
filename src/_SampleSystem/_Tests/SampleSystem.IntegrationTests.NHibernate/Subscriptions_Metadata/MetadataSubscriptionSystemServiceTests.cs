using System.Text;

using Framework.Core;
using Framework.Notification.Domain;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Models.Custom;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.Subscriptions_Metadata;

public sealed class MetadataSubscriptionSystemServiceTests : TestBase
{
    public MetadataSubscriptionSystemServiceTests() => this.GetNotifications().Clear();

    [Fact]
    public void SubscriptionFromMetadataShouldBeSent()
    {
        // Arrange
        var employee = this.CreateEmployee();

        // Act
        var results = this.DataHelper.ProcessSubscription(employee, employee);
        var errors = results.GetErrors().ToList();

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "SampleSystem@luxoft.com");

        // Assert
        Assert.Empty(errors);
        var notification = Assert.Single(expectedNotifications);
        Assert.Equal("replayTo@luxoft.com", Assert.Single(notification.Recipients, z => z.Type == RecipientRole.ReplyTo).Name);
    }

    [Fact]
    public void RazorTemplateImpl_SubscriptionFromMetadataShouldBeSent()
    {
        // Arrange
        var employee = this.DataHelper.SaveEmployee("Chuck Norris");
        var message = @"String.Concat it is good choice for Chuck Norris.";

        // Act
        var results = this.DataHelper.ProcessSubscription(employee, employee);
        var errors = results.GetErrors().ToList();

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "RazorTemplateImplSubscription@luxoft.com")
                                        .ToList();

        this.LogError(errors);

        // Assert
        Assert.Empty(errors);
        var notification = Assert.Single(expectedNotifications);
        Assert.Equal(message, notification.Message.Message);
        Assert.False(notification.Recipients.Any(z => z.Type == RecipientRole.ReplyTo));
    }

    [Fact]
    public void LocalRazorTemplate_SubscriptionFromMetadataShouldBeSent()
    {
        // Arrange
        var employee = this.CreateEmployee();
        var message = $"<h2>Hi there!!!</h2>{Environment.NewLine}My test employee Name:  John Doe {Environment.NewLine}Date: 21 Oct 2015";

        // Act
        var results = this.DataHelper.ProcessSubscription(employee, employee);
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
    [Fact]
    public void AttachTest()
    {
        // Arrange
        var employee = this.CreateEmployee();
        var content = Encoding.UTF8.GetBytes("Hello world!");

        // Act
        this.DataHelper.ProcessSubscription(employee, employee);

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "Attachment@luxoft.com");

        // Assert
        var notification = expectedNotifications.Single();
        var attachment = notification.Attachments.Single();
        Assert.Equal(content, attachment.Content);
        Assert.Equal(SampleSystem.Subscriptions.Metadata.Examples.Attachment.AttachmentSubscription.AttachmentName, attachment.Name);
    }

    /// <summary>
    /// IADFRAME-1525 Сделать пример использования аттачей в CodeFirst подписках
    /// </summary>
    /// <remarks>Создать тест: подписка с аттачем который провернут через шаблонизатор (TemplateEvaluatorFactory) просто текст, который добавляется в нотификацию</remarks>
    [Fact]
    public void AttachTemplateEvaluatorTest()
    {
        // Arrange
        var employee = this.CreateEmployee();
        var content = "Hello world!  John Doe ";

        // Act
        this.DataHelper.ProcessSubscription(employee, employee);

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
    [Fact]
    public void AttachInlinedTest()
    {
        // Arrange
        var employee = this.CreateEmployee();
        var messageTemplate = @"<html><head><title></title></head><body> John Doe <br/><img src=""cid:testId@luxoft.com""/></body></html>";

        // Act
        this.DataHelper.ProcessSubscription(employee, employee);

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "InlineAttach@luxoft.com");

        // Assert
        var notification = expectedNotifications.Single();
        var message = string.Format(messageTemplate, notification.Attachments[0].ContentId);
        Assert.Equal(message, notification.Message.Message);
        Assert.Single(notification.Attachments);
    }

    [Fact]
    public void DateModelCreateSubscriptionTest()
    {
        // Arrange

        // Act
        this.DataHelper.ProcessSubscription(null, new DateModel { Year = 2019 });

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "DateModelCreateSampleSystem@luxoft.com");

        // Assert
        Assert.Single(expectedNotifications);
    }

    private Employee CreateEmployee()
    {
        var employee = this.DataHelper.SaveEmployee("John Doe");
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
