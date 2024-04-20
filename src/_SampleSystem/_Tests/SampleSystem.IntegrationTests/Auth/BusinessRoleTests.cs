using FluentAssertions;

using Framework.Authorization.Generated.DTO;
using Framework.Exceptions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;

namespace SampleSystem.IntegrationTests.Auth;

[TestClass]
public class BusinessRoleTests : TestBase
{
    private const string NewDescription = "test_description";

    private static readonly string RoleName = SampleSystemSecurityRole.SeManager.Name;


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
        var businessRoleSimple = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleBusinessRole(businessRoleIdentity));

        businessRoleSimple.Name.Should().Be(RoleName);
        businessRoleSimple.Description.Should().Be(NewDescription);
        businessRoleSimple.Active.Should().BeTrue();
        businessRoleSimple.ModifiedBy.Should().Be(currentUser.Login.ToString());
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
}
