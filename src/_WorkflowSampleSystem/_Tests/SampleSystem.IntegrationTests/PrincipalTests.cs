using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.Events;
using Framework.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class PrincipalTests : TestBase
    {
        [TestMethod]
        public void CreatePrincipal_SaveEventExist()
        {
            // Arrange
            var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";

            // Act
            var principalIdentity = this.AuthHelper.SavePrincipal(name, true);

            // Assert
            this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PrincipalSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Principal.Id == principalIdentity.Id);
        }

        [TestMethod]
        public void CreatePrincipal_ForceEventExist()
        {
            // Arrange
            var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";
            var principalIdentity = this.AuthHelper.SavePrincipal(name, true);

            var configFacade = this.GetConfigurationController();

            var domainTypeIdentity = configFacade.GetSimpleDomainTypeByPath("Authorization/Principal").Identity;

            var domainType = configFacade.GetRichDomainType(domainTypeIdentity);

            var operation = domainType.EventOperations.Single(op => op.Name == nameof(EventOperation.Save));

            this.ClearIntegrationEvents();

            // Act
            configFacade.ForceDomainTypeEvent(new Framework.Configuration.Generated.DTO.DomainTypeEventModelStrictDTO
            {
                Operation = operation.Identity,

                DomainObjectIdents = new List<Guid> { principalIdentity.Id }
            });

            // Assert
            this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PrincipalSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Principal.Id == principalIdentity.Id);
        }

        [TestMethod]
        public void CreatePermission_ForceDependencyEventExist()
        {
            // Arrange
            var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";
            var principalIdentity = this.AuthHelper.SavePrincipal(name, true);

            var permissionIdentity = this.GetAuthorizationController().SavePermission(new AuthSLJsonController.SavePermissionAutoRequest(principalIdentity, new PermissionStrictDTO
            {
                Role = this.GetAuthorizationController().GetVisualBusinessRoleByName(Framework.Authorization.Domain.BusinessRole.AdminRoleName).Identity,
                Period = Period.Eternity
            }));

            var configFacade = this.GetConfigurationController();

            var domainTypeIdentity = configFacade.GetSimpleDomainTypeByPath("Authorization/Permission").Identity;

            var domainType = configFacade.GetRichDomainType(domainTypeIdentity);

            var operation = domainType.EventOperations.Single(op => op.Name == nameof(EventOperation.Save));

            this.ClearIntegrationEvents();

            // Act
            configFacade.ForceDomainTypeEvent(new Framework.Configuration.Generated.DTO.DomainTypeEventModelStrictDTO
            {
                Operation = operation.Identity,

                DomainObjectIdents = new List<Guid> { permissionIdentity.Id }
            });

            // Assert
            this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PermissionSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Permission.Id == permissionIdentity.Id);
            this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PrincipalSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Principal.Id == principalIdentity.Id);
        }

        [TestMethod]
        public void CreateActiveWithSameName_ValidationError()
        {
            // Arrange
            const string Name = @"luxoft\principaltest";

            this.AuthHelper.SavePrincipal(Name, true);

            // Act
            Action call = () => this.AuthHelper.SavePrincipal(Name, true);

            // Assert
            call.Should().Throw<ValidationException>().WithMessage("Active principal with name '*' already exists.");
        }

        [TestMethod]
        public void SaveWithExternalId_ExternalIdSaved()
        {
            // Arrange
            var expected = Guid.NewGuid();
            const string Name = @"luxoft\principaltest";

            // Act
            var principalId = this.AuthHelper.SavePrincipal(Name, true, expected);

            var principal = this.GetAuthorizationController().GetSimplePrincipal(principalId);

            // Assert
            principal.ExternalId.Should().Be(expected);
        }

        /// <summary>
        /// #IADFRAME-1300
        /// Если в основной системе есть объект Principal c полей ExternalId:string
        /// То при генерации базы данных аудита получается PrincipalAudit.ExternalId : guid
        /// Так как такая точно таблица и поле есть в Auth
        /// </summary>
        //[TestMethod]
        //public void SaveDomainPrincipalWithCustomExternalId_PrincipalSavedAndCreateAuditRecord()
        //{
        //    // Arrange
        //    var expected = StringUtil.RandomString("ExternalId_", 50);
        //    var model = new PrincipalStrictDTO
        //    {
        //        ExternalId = expected
        //    };

        //    // Act
        //    var principalId = this.GetAuthorizationController().SavePrincipal(model);
        //    var principal = this.GetAuthorizationController().GetSimplePrincipal(principalId);

        //    // Assert
        //    principal.ExternalId.Should().Be(expected);
        //}
    }
}
