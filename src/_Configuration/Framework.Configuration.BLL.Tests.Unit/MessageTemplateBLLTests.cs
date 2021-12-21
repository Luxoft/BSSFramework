using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using FluentAssertions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;
using Framework.UnitTesting;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;
using NSubstitute;

namespace Framework.Configuration.BLL.Tests.Unit
{
    [TestFixture]
    public sealed class MessageTemplateBLLTests
        : TestFixtureBase
    {
        private IMessageTemplateBLL bll;

        [SetUp]
        public void SetUp()
        {
            var configurationBLLContext = this.Fixture.RegisterStub<IConfigurationBLLContext>();

            var securityProvider = this.Fixture.RegisterStub<ISecurityProvider<MessageTemplate>>();
            securityProvider.HasAccess(Arg.Any<MessageTemplate>()).Returns(true);

            var securityService = this.Fixture.RegisterStub<IConfigurationSecurityService>();
            securityService.GetSecurityProvider<MessageTemplate>(BLLSecurityMode.Disabled).Returns(securityProvider);
            configurationBLLContext.SecurityService.Returns(securityService);

            var dal = this.Fixture.RegisterStub<IDAL<MessageTemplate, Guid>>();
            var dalFactory = this.Fixture.RegisterStub<IDALFactory<PersistentDomainObjectBase, Guid>>();
            dalFactory.CreateDAL<MessageTemplate>().Returns(dal);
            configurationBLLContext.DalFactory.Returns(dalFactory);

            var serviceProvider = new ServiceCollection().AddScoped(_ => configurationBLLContext)
                .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory)
                .BuildServiceProvider();

            configurationBLLContext.ServiceProvider.Returns(serviceProvider);

            this.bll = new MessageTemplateBLLFactory(configurationBLLContext).Create();
        }

        [Test]
        public void CreateMailMessage_FromTemplateWithAttachments_ShouldEnrichAttachmentsWithContentId()
        {
            // Arrange
            var attachmentName = "attachment.doc";
            var attachment = new System.Net.Mail.Attachment(new MemoryStream(Encoding.UTF8.GetBytes("attachment body")), attachmentName);

            // Act
            var result = this.bll.CreateMailMessage(
                "subject",
                "message text",
                new MailAddress("test@unit.com"),
                new List<TargetEmail>(),
                new List<TargetEmail>(),
                new List<TargetEmail>(),
                new List<System.Net.Mail.Attachment> { attachment });

            // Assert
            result.Attachments.Should().HaveCount(1)
                .And.OnlyContain(x => x.Name == attachmentName && !string.IsNullOrEmpty(x.ContentId));
        }

        [Test]
        public void CreateMailMessage_ForHtmlTemplateWithInlinedImages_ShouldReferenceToImagesByContentId()
        {
            // Arrange
            var attachmentName = "picture.jpg";
            var attachment = new System.Net.Mail.Attachment(new MemoryStream(Encoding.UTF8.GetBytes("picture body")), attachmentName);

            var htmlBody = $"<html><body><p>some text here <img src = \"{attachmentName}\" height = \"350\"/></html>";

            // Act
            var result = this.bll.CreateMailMessage(
                "subject",
                htmlBody,
                new MailAddress("test@unit.com"),
                new List<TargetEmail>(),
                new List<TargetEmail>(),
                new List<TargetEmail>(),
                new List<System.Net.Mail.Attachment> { attachment });

            var contentId = result.Attachments.FirstOrDefault()?.ContentId;

            // Assert
            result.Body.Should().Contain($"<img src=\"cid:{contentId}\"");
        }

        [Test]
        public void CreateMailMessage_CaseIgnoredWhenImagesInlined_ShouldReferenceToImagesByContentId()
        {
            // Arrange
            var attachmentName = "picture.jpg";
            var attachment = new System.Net.Mail.Attachment(new MemoryStream(Encoding.UTF8.GetBytes("picture body")), attachmentName);

            var htmlBody = $"<html><body><p>some text here <img src = \"{attachmentName.ToUpper()}\" height = \"350\"/></html>";

            // Act
            var result = this.bll.CreateMailMessage(
                "subject",
                htmlBody,
                new MailAddress("test@unit.com"),
                new List<TargetEmail>(),
                new List<TargetEmail>(),
                new List<TargetEmail>(),
                new List<System.Net.Mail.Attachment> { attachment });

            var contentId = result.Attachments.FirstOrDefault()?.ContentId;

            // Assert
            result.Body.Should().Contain($"<img src=\"cid:{contentId}\"");
        }
    }
}
