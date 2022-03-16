using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AttachmentsSampleSystem.Domain;
using AttachmentsSampleSystem.IntegrationTests.__Support.TestData;
using AttachmentsSampleSystem.WebApiCore.Controllers;

using Framework.Attachments.Generated.DTO;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class AttachmentTests : TestBase
    {
        [TestMethod]
        public void GetLocationAttachmentSecurityProviders_ProvidersEquals()
        {
            // Arrange

            // Act
            var providersEqual = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, context =>
            {
                var mainProv = context.SecurityService.GetSecurityProvider<Location>(BLLSecurityMode.View);

                var attachmentProv = context.SecurityService.GetAttachmentSecurityProvider<Location>(BLLSecurityMode.View);

                return mainProv == attachmentProv;
            });

            providersEqual.Should().Be(true);
        }

        [TestMethod]
        public void GetCountryAttachmentSecurityProviders_ProvidersNotEquals()
        {
            // Arrange

            // Act
            var providersEqual = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, context =>
            {
                var mainProv = context.SecurityService.GetSecurityProvider<Employee>(BLLSecurityMode.View);

                var attachmentProv = context.SecurityService.GetAttachmentSecurityProvider<Employee>(BLLSecurityMode.View);

                return mainProv == attachmentProv;
            });

            providersEqual.Should().Be(false);
        }

        [TestMethod]
        public void SaveAttachment_AttachmentExists()
        {
            // Arrange
            var employeeIdent = this.DataHelper.SaveEmployee();

            var sourceAttachment = new AttachmentStrictDTO
            {
                Name = "TestFile",
                Content = new byte[] { 1, 2, 3 }
            };

            var attachmentController = this.GetAttachmentControllerEvaluator();

            // Act
            var attachmentIdentity = attachmentController.Evaluate(c => c.SaveAttachment(nameof(Employee), employeeIdent.Id, sourceAttachment));

            var reloadedAttachment = attachmentController.Evaluate(c => c.GetRichAttachment(attachmentIdentity));

            // Assert
            reloadedAttachment.Name.SequenceEqual(sourceAttachment.Name).Should().Be(true);
            reloadedAttachment.Content.SequenceEqual(sourceAttachment.Content).Should().Be(true);
        }
    }
}
