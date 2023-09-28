using FluentAssertions;

using Framework.Authorization.Generated.DTO;
using Framework.Exceptions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.Auth;

[TestClass]
public class BusinessRoleTests : TestBase
{
    private const string NewDescription = "test_description";
    private const string RoleName = "SE Manager";

    [TestMethod]
    public void SaveBusinessRole_CheckOperationAddition()
    {
        // Arrange
        var authController = this.GetAuthControllerEvaluator();
        var employeeController = this.MainWebApi.Employee;
        var currentUser = employeeController.Evaluate(c=> c.GetFullEmployee(this.DataHelper.GetEmployeeByLogin(this.AuthHelper.GetCurrentUserLogin())));

        var operation = authController.Evaluate(c => c.GetSimpleOperationByName(SampleSystemSecurityOperation.BusinessUnitTypeModuleOpen.ToString()));

        var oprManager = authController.Evaluate(c => c.GetRichBusinessRoleByName(RoleName));
        oprManager.BusinessRoleOperationLinks.Add(new BusinessRoleOperationLinkRichDTO
                                                  {
                                                          BusinessRole = oprManager,
                                                          Operation = operation,
                                                          IsDenormalized = false
                                                  });

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(oprManager.ToStrict()));

        // Assert
        var roleOperationLink = authController.Evaluate(c => c.GetRichBusinessRoleByName(RoleName))
                                              .BusinessRoleOperationLinks.First(x => x.Operation.Identity == operation.Identity);

        roleOperationLink.BusinessRole.Identity.Should().Be(oprManager.Identity);
        roleOperationLink.CreatedBy.Should().Be(currentUser.Login.ToString());
        roleOperationLink.ModifiedBy.Should().Be(currentUser.Login.ToString());
        roleOperationLink.Active.Should().BeTrue();
    }

    [TestMethod]
    public void SaveBusinessRole_CheckSubBusinessRoleAddition()
    {
        // Arrange
        var currentUser = this.DataHelper.GetCurrentEmployee();

        var subRole = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRoleByName("SecretariatNotification"));

        var oprManager = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName(RoleName));
        oprManager.SubBusinessRoleLinks.Add(new SubBusinessRoleLinkRichDTO
                                            {
                                                    BusinessRole = oprManager,
                                                    SubBusinessRole = subRole,
                                            });

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(oprManager.ToStrict()));

        // Assert
        var subRoleLink = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName(RoleName)).SubBusinessRoleLinks
                              .First(x => x.SubBusinessRole.Identity == subRole.Identity);

        subRoleLink.BusinessRole.Identity.Should().Be(oprManager.Identity);
        subRoleLink.CreatedBy.Should().Be(currentUser.Login.ToString());
        subRoleLink.ModifiedBy.Should().Be(currentUser.Login.ToString());
        subRoleLink.Active.Should().BeTrue();
    }

    [TestMethod]
    public void SaveBusinessRole_CheckCreation()
    {
        // Arrange
        const string businessRoleName = "MyBusinessRole111";
        var currentUser = this.DataHelper.GetCurrentEmployee();

        var businessRoleStrict = new BusinessRoleStrictDTO
                                 {
                                         Name = businessRoleName
                                 };

        // Act
        var businessRoleIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(businessRoleStrict));

        // Assert
        var businessRoleSimple = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRole(businessRoleIdentity));

        businessRoleSimple.Name.Should().Be(businessRoleName);
        businessRoleSimple.Active.Should().BeTrue();
        businessRoleSimple.CreatedBy.Should().Be(currentUser.Login.ToString());
        businessRoleSimple.ModifiedBy.Should().Be(currentUser.Login.ToString());
    }

    [TestMethod]
    public void SaveBusinessRole_CheckBusinessRoleChanges()
    {
        // Arrange
        var currentUser = this.DataHelper.GetCurrentEmployee();

        var businessRoleIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRoleByName(RoleName)).Identity;
        var businessRoleStrict = this.GetAuthControllerEvaluator().Evaluate(c => c.GetFullBusinessRole(businessRoleIdentity)).ToStrict();
        businessRoleStrict.Description = NewDescription;

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(businessRoleStrict));

        // Assert
        var businessRoleSiple = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRole(businessRoleIdentity));

        businessRoleSiple.Name.Should().Be(RoleName);
        businessRoleSiple.Description.Should().Be(NewDescription);
        businessRoleSiple.Active.Should().BeTrue();
        businessRoleSiple.ModifiedBy.Should().Be(currentUser.Login.ToString());
    }

    [TestMethod]
    public void SaveBusinessRole_CheckOperationRemoval()
    {
        // Arrange
        var operationIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleOperationByName(SampleSystemSecurityOperation.EmployeeView.Name)).Identity;

        var oprManager = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName(RoleName));

        var operationLinkIdentity = oprManager.BusinessRoleOperationLinks
                                              .Single(x => x.Operation.Identity == operationIdentity).Identity;

        oprManager.BusinessRoleOperationLinks = oprManager.BusinessRoleOperationLinks.Where(
         x => x.Operation.Identity != operationIdentity).ToList();

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(oprManager.ToStrict()));

        // Assert
        oprManager = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName(RoleName));
        oprManager.BusinessRoleOperationLinks.Should().NotContain(x => x.Identity == operationLinkIdentity);
    }

    [TestMethod]
    public void RemoveBusinessRole_CheckRemoval()
    {
        // Arrange
        const string businessRoleName = "MyBusinessRole111";
        var businessRoleStrict = new BusinessRoleStrictDTO { Name = businessRoleName };

        var businessRoleIdentity = this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(businessRoleStrict));

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.RemoveBusinessRole(businessRoleIdentity));

        Action call = () => this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRole(businessRoleIdentity));

        // Assert
        call.Should().Throw<ObjectByIdNotFoundException<Guid>>().WithMessage("BusinessRole with id = * not found");
    }

    [TestMethod]
    public void SaveBusinessRole_CheckSubBusinessRoleRemoval()
    {
        // Arrange
        var subRole = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRoleByName("SecretariatNotification"));

        var oprManager = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName(RoleName));
        oprManager.SubBusinessRoleLinks.Add(new SubBusinessRoleLinkRichDTO
                                            {
                                                    BusinessRole = oprManager,
                                                    SubBusinessRole = subRole,
                                            });

        this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(oprManager.ToStrict()));
        oprManager = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName(RoleName));

        var subRoleLinkIdentity = oprManager.SubBusinessRoleLinks
                                            .Single(x => x.SubBusinessRole.Identity == subRole.Identity).Identity;

        oprManager.SubBusinessRoleLinks = oprManager.SubBusinessRoleLinks
                                                    .Where(x => x.SubBusinessRole.Identity != subRole.Identity).ToList();

        // Act
        this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(oprManager.ToStrict()));

        // Assert
        oprManager = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName(RoleName));
        oprManager.SubBusinessRoleLinks.Should().NotContain(x => x.Identity == subRoleLinkIdentity);
    }

    [TestMethod]
    public void RemoveBusinessRoleWithOperations_CheckException()
    {
        // Arrange
        var role = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName("SecretariatNotification")).ToStrict();
        role.BusinessRoleOperationLinks.Add(new BusinessRoleOperationLinkStrictDTO
                                            {
                                                Operation = new OperationIdentityDTO(SampleSystemSecurityOperation.EmployeeView.Id)
                                            });

        this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(role));

        // Act
        Action call = () => this.GetAuthControllerEvaluator().Evaluate(c => c.RemoveBusinessRole(role.Identity));

        // Assert
        call.Should().Throw<Exception>().WithMessage("Removing business role \"SecretariatNotification\" must be empty");
    }
}
