using Framework.Authorization.Generated.DTO;
using Framework.BLL.Exceptions;

using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.Main;

using Anch.SecuritySystem;

using SampleSystem.IntegrationTests._Environment.TestData;

using DelegateToItemModelStrictDTO = Framework.Authorization.Generated.DTO.DelegateToItemModelStrictDTO;

namespace SampleSystem.IntegrationTests.Auth;

public class PrincipalTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    private const string Name = "luxoft\\Login";
    private const string NewName = "luxoft\\ChangeLogin";

    [Fact]
    public void AddPermission_CheckAddition()
    {
        // Arrange
        var authorizationController = this.GetAuthControllerEvaluator();
        var currentUser = this.DataManager.GetCurrentEmployee();

        var businessRoleIdentity = authorizationController.Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalIdentity = authorizationController.Evaluate(c => c.GetCurrentPrincipal()).Identity;

        var permissionStrict = new PermissionStrictDTO { Role = businessRoleIdentity };

        // Act
        var saveRequest = new AuthMainController.SavePermissionAutoRequest(principalIdentity, permissionStrict);
        var permissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

        // Assert
        var permissionSimple = authorizationController.Evaluate(c => c.GetSimplePermission(permissionIdentity));
        Assert.Equal(currentUser.Login.ToString(), permissionSimple.CreatedBy);
        Assert.Equal(currentUser.Login.ToString(), permissionSimple.ModifiedBy);
    }

    [Fact]
    public void SavePrincipal_CheckCreateon()
    {
        // Arrange
        var authorizationController = this.GetAuthControllerEvaluator();
        var currentUser = this.DataManager.GetCurrentEmployee();

        var businessRoleIdentity = authorizationController.Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalStrict = new PrincipalStrictDTO
                              {
                                      Name = Name,
                                      Permissions = [new PermissionStrictDTO { Role = businessRoleIdentity }]
                              };

        // Act
        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));

        // Assert
        var principalRich = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichPrincipal(principalIdentity));

        Assert.Equal(Name, principalRich.Name);
        Assert.Equal(currentUser.Login.ToString(), principalRich.CreatedBy);
        Assert.Equal(currentUser.Login.ToString(), principalRich.ModifiedBy);
        Assert.Equal(businessRoleIdentity, principalRich.Permissions.First().Role.Identity);
    }

    [Fact]
    public void SavePrincipal_CheckPrincipalChanges()
    {
        // Arrange
        var currentUser = this.DataManager.GetCurrentEmployee();

        var principalStrict = new PrincipalStrictDTO { Name = Name };
        this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));
        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipalByName(Name)).Identity;

        principalStrict = this.GetAuthControllerEvaluator().Evaluate(c => c.GetFullPrincipal(principalIdentity)).ToStrict();
        principalStrict.Name = NewName;

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));

        // Assert
        var principalSimple = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipal(principalStrict.Identity));

        Assert.Equal(NewName, principalSimple.Name);
        Assert.Equal(currentUser.Login.ToString(), principalSimple.ModifiedBy);
    }

    [Fact]
    public void PermissionDelegate_CheckChanges()
    {
        // Arrange
        var currentUser = this.DataManager.GetCurrentEmployee();

        var businessRoleIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetCurrentPrincipal()).Identity;

        var permissionStrict = new PermissionStrictDTO { Role = businessRoleIdentity };

        var saveRequest = new AuthMainController.SavePermissionAutoRequest(principalIdentity, permissionStrict);
        var permissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

        var newPrincipalStrict = new PrincipalStrictDTO { Name = Name };
        var newPrincipalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(newPrincipalStrict));

        var changePermissionDelegate = new ChangePermissionDelegatesModelStrictDTO
                                       {
                                               DelegateFromPermission = permissionIdentity,
                                               Items =
                                               {
                                                       new DelegateToItemModelStrictDTO
                                                       {
                                                               Principal = newPrincipalIdentity,
                                                               Permission = new PermissionStrictDTO { Role = businessRoleIdentity }
                                                       }
                                               }
                                       };

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.ChangeDelegatePermissions(changePermissionDelegate));

        // Assert
        var newPermissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetFullPermissions()
                                                                                     .Single(x => x.Principal.Identity == newPrincipalIdentity)).Identity;

        var newPermissionFull = this.GetAuthControllerEvaluator().Evaluate(c => c.GetFullPermission(newPermissionIdentity));
        Assert.Equal(currentUser.Login.ToString(), newPermissionFull.CreatedBy);
        Assert.Equal(currentUser.Login.ToString(), newPermissionFull.ModifiedBy);

        var permissionSimple = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichPermission(permissionIdentity));
        Assert.True(permissionSimple.DelegatedTo.Any());
    }

    [Fact]
    public void RemovePermission_CheckRemoval()
    {
        // Arrange
        var businessRoleIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetCurrentPrincipal()).Identity;

        var permissionStrict = new PermissionStrictDTO { Role = businessRoleIdentity };

        var saveRequest = new AuthMainController.SavePermissionAutoRequest(principalIdentity, permissionStrict);
        var permissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.RemovePermission(permissionIdentity));
        var ex = Record.Exception(() => this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePermission(permissionIdentity)));

        // Assert
        var notFoundException = Assert.IsType<ObjectByIdNotFoundException<Guid>>(ex);
        Assert.Matches("^Permission with id = \".*\" not found$", notFoundException.Message);
    }

    [Fact]
    public void RemovePrincipalWithRole_CheckException()
    {
        // Arrange
        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetCurrentPrincipal()).Identity;

        // Act
        var ex = Record.Exception(() => this.GetAuthControllerEvaluator().Evaluate(c => c.RemovePrincipal(principalIdentity)));

        // Assert
        var securityException = Assert.IsType<SecuritySystemException>(ex);
        Assert.Matches("^Removing principal \".*\" must be empty$", securityException.Message);
    }

    [Fact]
    public void RemovePrincipal_CheckRemoval()
    {
        // Arrange
        var principalStrict = new PrincipalStrictDTO { Name = Name };

        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.RemovePrincipal(principalIdentity));
        var ex = Record.Exception(() => this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipal(principalIdentity)));

        // Assert
        var notFoundException = Assert.IsType<ObjectByIdNotFoundException<Guid>>(ex);
        Assert.Matches("^Principal with id = \".*\" not found$", notFoundException.Message);
    }
}
