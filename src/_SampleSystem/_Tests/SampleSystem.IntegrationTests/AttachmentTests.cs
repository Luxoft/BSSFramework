using System;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers;

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
                var mainProv = context.SecurityService.GetSecurityProvider<Country>(BLLSecurityMode.View);

                var attachmentProv = context.SecurityService.GetAttachmentSecurityProvider<Country>(BLLSecurityMode.View);

                return mainProv == attachmentProv;
            });

            providersEqual.Should().Be(false);
        }
    }
}
