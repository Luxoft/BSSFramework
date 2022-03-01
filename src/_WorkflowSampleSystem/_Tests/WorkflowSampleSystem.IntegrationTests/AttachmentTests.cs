using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.WebApiCore.Controllers;

namespace WorkflowSampleSystem.IntegrationTests
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
                var mainProv = context.SecurityService.GetSecurityProvider<Country>(BLLSecurityMode.View);

                var attachmentProv = context.SecurityService.GetAttachmentSecurityProvider<Country>(BLLSecurityMode.View);

                return mainProv == attachmentProv;
            });

            providersEqual.Should().Be(false);
        }

        [TestMethod]
        public void SaveAttachment_AttachmentExists()
        {
            // Arrange
            var countryId = this.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Write,
                context =>
                {
                    var country = new Country
                    {
                        Name = Guid.NewGuid().ToString(),
                        NameNative = Guid.NewGuid().ToString(),
                        Code = Guid.NewGuid().ToString(),
                        Culture = Guid.NewGuid().ToString()
                    };

                    context.Logics.Country.Save(country);

                    return country.Id;
                });

            var sourceAttachment = new AttachmentStrictDTO
            {
                Name = "TestFile",
                Content = new byte[] { 1, 2, 3 }
            };

            var facade = this.GetConfigurationController();

            // Act
            var attachmentIdentity = facade.SaveAttachment(nameof(Country), countryId, sourceAttachment);

            var reloadedAttachment = facade.GetRichAttachment(attachmentIdentity);

            // Assert
            reloadedAttachment.Name.SequenceEqual(sourceAttachment.Name).Should().Be(true);
            reloadedAttachment.Content.SequenceEqual(sourceAttachment.Content).Should().Be(true);
        }
    }
}
