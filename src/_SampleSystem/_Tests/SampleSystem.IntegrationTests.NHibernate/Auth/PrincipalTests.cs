﻿using Framework.Authorization.Generated.DTO;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers;

namespace SampleSystem.IntegrationTests.Auth;

[TestClass]
public class PrincipalTests : TestBase
{
    private const string Name = "luxoft\\Login";
    private const string NewName = "luxoft\\ChangeLogin";

    [TestMethod]
    public void AddPermission_CheckAddition()
    {
        // Arrange
        var authorizationController = this.GetAuthControllerEvaluator();
        var currentUser = this.DataHelper.GetCurrentEmployee();

        var businessRoleIdentity = authorizationController.Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalIdentity = authorizationController.Evaluate(c => c.GetCurrentPrincipal()).Identity;

        var permissionStrict = new PermissionStrictDTO { Role = businessRoleIdentity };

        // Act
        var saveRequest = new AuthSLJsonController.SavePermissionAutoRequest(principalIdentity, permissionStrict);
        var permissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

        // Assert
        var permissionSimple = authorizationController.Evaluate(c => c.GetSimplePermission(permissionIdentity));
        permissionSimple.CreatedBy.Should().Be(currentUser.Login.ToString());
        permissionSimple.ModifiedBy.Should().Be(currentUser.Login.ToString());
    }

    [TestMethod]
    public void SavePrincipal_CheckCreateon()
    {
        // Arrange
        var authorizationController = this.GetAuthControllerEvaluator();
        var currentUser = this.DataHelper.GetCurrentEmployee();

        var businessRoleIdentity = authorizationController.Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalStrict = new PrincipalStrictDTO
                              {
                                      Name = Name,
                                      Permissions = new List<PermissionStrictDTO>
                                                    {
                                                            new PermissionStrictDTO { Role = businessRoleIdentity }
                                                    }
                              };

        // Act
        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));

        // Assert
        var principalRich = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichPrincipal(principalIdentity));

        principalRich.Name.Should().Be(Name);
        principalRich.CreatedBy.Should().Be(currentUser.Login.ToString());
        principalRich.ModifiedBy.Should().Be(currentUser.Login.ToString());
        principalRich.Permissions.First().Role.Identity.Should().Be(businessRoleIdentity);
    }

    [TestMethod]
    public void SavePrincipal_CheckPrincipalChanges()
    {
        // Arrange
        var currentUser = this.DataHelper.GetCurrentEmployee();

        var principalStrict = new PrincipalStrictDTO { Name = Name };
        this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));
        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipalByName(Name)).Identity;

        principalStrict = this.GetAuthControllerEvaluator().Evaluate(c => c.GetFullPrincipal(principalIdentity)).ToStrict();
        principalStrict.Name = NewName;

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));

        // Assert
        var principalSimple = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipal(principalStrict.Identity));

        principalSimple.Name.Should().Be(NewName);
        principalSimple.ModifiedBy.Should().Be(currentUser.Login.ToString());
    }

    [TestMethod]
    public void PermissionDelegate_CheckChanges()
    {
        // Arrange
        var currentUser = this.DataHelper.GetCurrentEmployee();

        var businessRoleIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetCurrentPrincipal()).Identity;

        var permissionStrict = new PermissionStrictDTO { Role = businessRoleIdentity };

        var saveRequest = new AuthSLJsonController.SavePermissionAutoRequest(principalIdentity, permissionStrict);
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
        newPermissionFull.CreatedBy.Should().Be(currentUser.Login.ToString());
        newPermissionFull.ModifiedBy.Should().Be(currentUser.Login.ToString());

        var permissionSimple = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichPermission(permissionIdentity));
        permissionSimple.DelegatedTo.Any().Should().BeTrue();
    }

    [TestMethod]
    public void RemovePermission_CheckRemoval()
    {
        // Arrange
        var businessRoleIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRoleByName(SampleSystemSecurityRole.SecretariatNotification.Name)).Identity;

        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetCurrentPrincipal()).Identity;

        var permissionStrict = new PermissionStrictDTO { Role = businessRoleIdentity };

        var saveRequest = new AuthSLJsonController.SavePermissionAutoRequest(principalIdentity, permissionStrict);
        var permissionIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.RemovePermission(permissionIdentity));
        Action call = () => this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePermission(permissionIdentity));

        // Assert
        call.Should().Throw<Exception>().WithMessage("Permission with id = \"*\" not found");
    }

    [TestMethod]
    public void RemovePrincipalWithRole_CheckException()
    {
        // Arrange
        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetCurrentPrincipal()).Identity;

        // Act
        var call = () => this.GetAuthControllerEvaluator().Evaluate(c => c.RemovePrincipal(principalIdentity));

        // Assert
        call.Should().Throw<Exception>().WithMessage("Removing principal \"*\" must be empty");
    }

    [TestMethod]
    public void RemovePrincipal_CheckRemoval()
    {
        // Arrange
        var principalStrict = new PrincipalStrictDTO { Name = Name };

        var principalIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SavePrincipal(principalStrict));

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.RemovePrincipal(principalIdentity));
        var call = () => this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimplePrincipal(principalIdentity));

        // Assert
        call.Should().Throw<Exception>().WithMessage("Principal with id = \"*\" not found");
    }
}
