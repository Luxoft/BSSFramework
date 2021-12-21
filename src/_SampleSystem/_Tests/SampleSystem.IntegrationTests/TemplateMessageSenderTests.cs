using System.Linq;
using System.Text;
using FluentAssertions;
using Framework.Configuration.Domain;
using Framework.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public sealed class TemplateMessageSenderTests
        : TestBase
    {
        private const string TestRecipientTo = "subscription_tester@luxoft.com";

        private const string TestRecipientCc = "subscription_tester_cc@luxoft.com";

        [TestInitialize]
        public void SetUp()
        {
            this.GetNotifications().Clear();
        }

        [TestMethod]
        public void ProcessChangedObjectUntyped_HtmlMessageTemplateWithImages_ShouldAssignContentIdToImagesAndUseItInMessageBody()
        {
            // Arrange
            var messageTemplate = "<html>"
                + "<body>"
                + "  <p>"
                + "      This is a Content-ID test. <img src=\"picture1.png\" height=\"350\"/>"
                + "      Attachments should be referenced by cid instead of name <img width=\"450\" src = \"picture2.jpg\"/>"
                + "      due to Outlook Web App specifics"
                + "  </p>"
                + "</body>"
                + "</html>";

            this.CreateSubscription(messageTemplate);

            var employee = this.DataHelper.SaveEmployee("John Doe");

            // Act
            this.DataHelper.ProcessChangedObjectUntyped(typeof(Employee), employee, employee);

            var expectedNotification = this.GetNotifications().SingleOrDefault(n => n.From == this.Environment.NotificationContext.Sender.Address);

            // Assert
            expectedNotification.Attachments.Where(x => x.Name.StartsWith("picture")).Foreach(
                attachment =>
                {
                    attachment.ContentId.Should().NotBeNullOrWhiteSpace();
                    expectedNotification.Message.Message.Should().Contain($"src=\"cid:{attachment.ContentId}\"");
                });
        }

        [TestMethod]
        public void ProcessChangedObjectUntyped_MessageWithNotImageAttachment_ShouldAssignContentIdToAttachment()
        {
            // Arrange
            var messageTemplate = "some useless text goes here";

            this.CreateSubscription(messageTemplate);

            var employee = this.DataHelper.SaveEmployee("John Doe");

            // Act
            this.DataHelper.ProcessChangedObjectUntyped(typeof(Employee), employee, employee);

            var expectedNotification = this.GetNotifications().SingleOrDefault(n => n.From == this.Environment.NotificationContext.Sender.Address);

            // Assert
            expectedNotification.Attachments.Where(x => x.Name.StartsWith("doc")).Foreach(
                attachment =>
                {
                    attachment.ContentId.Should().NotBeNullOrWhiteSpace();
                });
        }

        private void CreateSubscription(string template)
        {
            var messageTemplate = this.DataHelper.SaveMessageTemplate("T000", template);

            var attachmentsContainer = this.DataHelper.SaveAttachmentContainer(
                messageTemplate.Id,
                this.DataHelper.GetDomainType(typeof(MessageTemplate)));

            this.DataHelper.SaveAttachment(
                attachmentsContainer,
                "picture1.png",
                Encoding.UTF8.GetBytes("attachment body 1"));

            this.DataHelper.SaveAttachment(
                attachmentsContainer,
                "picture2.jpg",
                Encoding.UTF8.GetBytes("attachment body 2"));

            this.DataHelper.SaveAttachment(
                attachmentsContainer,
                "doc1.doc",
                Encoding.UTF8.GetBytes("attachment body 2"));

            var domainType = this.DataHelper.GetDomainType(typeof(Employee));

            var conditionLambda = this.DataHelper.SaveSubscriptionLambda(
                "CL000",
                SubscriptionLambdaType.Condition,
                domainType,
                "(prev, next) => true",
                false);

            var generationLambda = this.DataHelper.SaveSubscriptionLambda(
                "GL000",
                SubscriptionLambdaType.Generation,
                domainType,
                $"(p, c) => new List<NotificationMessageGenerationInfo>() {{new NotificationMessageGenerationInfo(\"{TestRecipientTo}\", c, p)}};",
                false);

            var copyGenerationLambda = this.DataHelper.SaveSubscriptionLambda(
                "GL001",
                SubscriptionLambdaType.Generation,
                domainType,
                $"(p, c) => new List<NotificationMessageGenerationInfo>() {{new NotificationMessageGenerationInfo(\"{TestRecipientCc}\", c, p)}};",
                false);

            this.DataHelper.SaveSubscription(
                "S000",
                true,
                domainType,
                messageTemplate,
                conditionLambda,
                generationLambda,
                copyGenerationLambda);
        }
    }
}
