using System.Text;

using Framework.Core;
using Framework.Notification.Domain;

using SampleSystem.Domain;
using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Models.Custom;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.Subscriptions_Metadata;

[TestClass]
public sealed class MetadataSubscriptionSystemServiceTests : TestBase
{
    [TestInitialize]
    public void SetUp() => this.GetNotifications().Clear();

    [TestMethod]
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
        errors.Should().HaveCount(0);
        expectedNotifications.Should().HaveCount(1);
        expectedNotifications.Single().Recipients.Single(z => z.Type == RecipientRole.ReplyTo).Name.Should().Be("replayTo@luxoft.com");
    }

    [TestMethod]
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
        errors.Should().HaveCount(0);
        expectedNotifications.Should().HaveCount(1);
        expectedNotifications.Single().Message.Message.Should().BeEquivalentTo(message);
        expectedNotifications.Single()
                             .Recipients.Any(z => z.Type == RecipientRole.ReplyTo)
                             .Should()
                             .BeFalse();
    }

    [TestMethod]
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
        errors.Should().HaveCount(0);
        expectedNotifications.Should().HaveCount(1);
        expectedNotifications.Single().Message.Message.Should().BeEquivalentTo(message);
    }

    /// <summary>
    /// IADFRAME-1525 Сделать пример использования аттачей в CodeFirst подписках
    /// </summary>
    /// <remarks>Создать тест: подписка с аттачем, который добавляется в нотификацию</remarks>
    [TestMethod]
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
        attachment.Content.Should().BeEquivalentTo(content);
        attachment.Name.Should().Be(SampleSystem.Subscriptions.Metadata.Examples.Attachment.AttachmentSubscription.AttachmentName);
    }

    /// <summary>
    /// IADFRAME-1525 Сделать пример использования аттачей в CodeFirst подписках
    /// </summary>
    /// <remarks>Создать тест: подписка с аттачем который провернут через шаблонизатор (TemplateEvaluatorFactory) просто текст, который добавляется в нотификацию</remarks>
    [TestMethod]
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
        Encoding.UTF8.GetString(attachment.Content).Should().BeEquivalentTo(content);
        attachment.Name.Should().Be(SampleSystem.Subscriptions.Metadata.Examples.AttachmentTemplateEvaluator.AttachmentTemplateEvaluatorSubscription.AttachmentName);
    }

    /// <summary>
    /// IADFRAME-1525 Сделать пример использования аттачей в CodeFirst подписках
    /// </summary>
    /// <remarks>Создать тест: подписка с inline аттачем (ContentId), который добавляется в нотификацию</remarks>
    [TestMethod]
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
        notification.Message.Message.Should().BeEquivalentTo(message);
        notification.Attachments.Should().HaveCount(1);
    }

    [TestMethod]
    public void DateModelCreateSubscriptionTest()
    {
        // Arrange

        // Act
        this.DataHelper.ProcessSubscription(null, new DateModel { Year = 2019 });

        var expectedNotifications = this.GetNotifications()
                                        .Where(n => n.From == "DateModelCreateSampleSystem@luxoft.com");

        // Assert
        expectedNotifications.Should().HaveCount(1);
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
