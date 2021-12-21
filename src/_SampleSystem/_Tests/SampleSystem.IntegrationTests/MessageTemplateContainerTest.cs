using System;
using System.Linq;

using FluentAssertions;

using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class MessageTemplateContainerTest : TestBase
    {
        [TestMethod]
        public void SaveConfigurationMessageTemplate_SaveMessageTemplateContainer()
        {
            // Arrange
            var sampleSystemMessageTemplateController = this.GetController<SampleSystemMessageTemplateController>();
            var messageTemplateContainerController = this.GetController<MessageTemplateContainerController>();
            var domainType = this.GetConfigurationController().GetSimpleDomainTypes().First();
            var messageTemplate = this.GetConfigurationController().SaveMessageTemplate(
                new MessageTemplateStrictDTO()
                {
                    Code = "code",
                    DomainType = domainType.Identity,
                    Renderer = TemplateRenderer.Razor
                });
            var templates = sampleSystemMessageTemplateController.GetSimpleSampleSystemMessageTemplates();

            // Act
            var result = messageTemplateContainerController.SaveMessageTemplateContainer(new MessageTemplateContainerStrictDTO() { MessageTemplate = templates.First().Identity });

            // Assert
            result.Id.Should().NotBe(Guid.Empty);
        }

    }
}
