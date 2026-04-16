using Framework.Application.Events;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;

using SampleSystem.IntegrationTests.__Support.TestData;

using SecuritySystem;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.IntegrationTests;

public class PrincipalTests : TestBase
{
    [Fact]
    public void CreatePrincipal_SaveEventExist()
    {
        // Arrange
        var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";

        // Act
        var principalId = (Guid)this.AuthManager.For(name).CreatePrincipal().GetId();

        // Assert
        Assert.Contains(this.GetIntegrationEvents<PrincipalSaveEventDTO>("authDALQuery"), dto => dto.Principal.Id == principalId);
    }

    [Fact]
    public void CreatePrincipal_ForceEventExist()
    {
        // Arrange
        var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";

        var principalId = (Guid)this.AuthManager.For(name).CreatePrincipal().GetId();

        var configFacade = this.GetConfigurationControllerEvaluator();

        var domainTypeIdentity = configFacade.Evaluate(c => c.GetSimpleDomainTypes())
                                             .Single(dt => dt.Namespace == typeof(Principal).Namespace && dt.Name == nameof(Principal))
                                             .Identity;

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
        Assert.Contains(this.GetIntegrationEvents<PrincipalSaveEventDTO>("authDALQuery"), dto => dto.Principal.Id == principalId);
    }

    [Fact]
    public void CreatePermission_ForceDependencyEventExist()
    {
        // Arrange
        var name = $@"luxoft\saveprincipaltest_{Guid.NewGuid()}";

        var principalId = (Guid)this.AuthManager.For(name).CreatePrincipal().GetId();

        var role = this.GetAuthControllerEvaluator().Evaluate(c => c.GetVisualBusinessRoleByName(SecurityRole.Administrator.Name)).Identity;

        var saveRequest = new AuthMainController.SavePermissionAutoRequest(new PrincipalIdentityDTO(principalId), new PermissionStrictDTO
                                                                                 {
                                                                                         Role = role,
                                                                                         Period = Period.Eternity
                                                                                 });
        var permissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

        var configFacade = this.GetConfigurationControllerEvaluator();

        var domainTypeIdentity = configFacade.Evaluate(c => c.GetSimpleDomainTypes())
                                             .Single(dt => dt.Namespace == typeof(Permission).Namespace && dt.Name == nameof(Permission))
                                             .Identity;

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
        Assert.Contains(this.GetIntegrationEvents<PermissionSaveEventDTO>("authDALQuery"), dto => dto.Permission.Id == permissionIdentity.Id);
        Assert.Contains(this.GetIntegrationEvents<PrincipalSaveEventDTO>("authDALQuery"), dto => dto.Principal.Id == principalId);
    }
}
