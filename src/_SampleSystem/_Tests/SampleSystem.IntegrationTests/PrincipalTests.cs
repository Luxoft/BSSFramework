using FluentAssertions;

using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.Events;
using Framework.SecuritySystem;
using Framework.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class PrincipalTests : TestBase
{
    [TestMethod]
    public void CreatePrincipal_SaveEventExist()
    {
        // Arrange
        var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";

        // Act
        var principalId = this.AuthHelper.SavePrincipal(name, true);

        // Assert
        this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PrincipalSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Principal.Id == principalId);
    }

    [TestMethod]
    public void CreatePrincipal_ForceEventExist()
    {
        // Arrange
        var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";

        var principalId = this.AuthHelper.SavePrincipal(name, true);

        var configFacade = this.GetConfigurationControllerEvaluator();

        var domainTypeIdentity = configFacade.Evaluate(c => c.GetSimpleDomainTypeByPath("Authorization/Principal")).Identity;

        var domainType = configFacade.Evaluate(c => c.GetRichDomainType(domainTypeIdentity));

        var operation = domainType.EventOperations.Single(op => op.Name == EventOperation.Save.Name);

        this.ClearIntegrationEvents();

        // Act
        configFacade.Evaluate(c => c.ForceDomainTypeEvent(new Framework.Configuration.Generated.DTO.DomainTypeEventModelStrictDTO
                                                          {
                                                                  Operation = operation.Identity,

                                                                  DomainObjectIdents = new List<Guid> { principalId }
                                                          }));

        // Assert
        this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PrincipalSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Principal.Id == principalId);
    }

    [TestMethod]
    public void CreatePermission_ForceDependencyEventExist()
    {
        // Arrange
        var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";

        var principalId = this.AuthHelper.SavePrincipal(name, true);

        var role = this.GetAuthControllerEvaluator().Evaluate(c => c.GetVisualBusinessRoleByName(SecurityRole.Administrator.Name)).Identity;

        var saveRequest = new AuthSLJsonController.SavePermissionAutoRequest(new PrincipalIdentityDTO(principalId), new PermissionStrictDTO
                                                                                 {
                                                                                         Role = role,
                                                                                         Period = Period.Eternity
                                                                                 });
        var permissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

        var configFacade = this.GetConfigurationControllerEvaluator();

        var domainTypeIdentity = configFacade.Evaluate(c => c.GetSimpleDomainTypeByPath("Authorization/Permission")).Identity;

        var domainType = configFacade.Evaluate(c => c.GetRichDomainType(domainTypeIdentity));

        var operation = domainType.EventOperations.Single(op => op.Name == EventOperation.Save.Name);

        this.ClearIntegrationEvents();

        // Act
        configFacade.Evaluate(c => c.ForceDomainTypeEvent(new Framework.Configuration.Generated.DTO.DomainTypeEventModelStrictDTO
                                                          {
                                                                  Operation = operation.Identity,

                                                                  DomainObjectIdents = new List<Guid> { permissionIdentity.Id }
                                                          }));

        // Assert
        this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PermissionSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Permission.Id == permissionIdentity.Id);
        this.GetIntegrationEvents<Framework.Authorization.Generated.DTO.PrincipalSaveEventDTO>("authDALQuery").Should().Contain(dto => dto.Principal.Id == principalId);
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

        var principal = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipal(new PrincipalIdentityDTO(principalId)));

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
    //    var principalId = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(model);
    //    var principal = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipal(principalId);

    //    // Assert
    //    principal.ExternalId.Should().Be(expected);
    //}
}
