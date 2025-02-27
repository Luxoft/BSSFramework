using FluentAssertions;

using Framework.DomainDriven.DALExceptions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class UniqueGroupTests : TestBase
{
    [TestMethod]
    public void UniqueGroup_NonUniqueEntityCreated_ErrorUsesCustomName()
    {
        // Arrange
        var role = this.DataHelper.SaveEmployeeRole();
        var roleDegree = this.DataHelper.SaveEmployeeRoleDegree();

        this.DataHelper.SaveRoleRoleDegreeLink(role, roleDegree);

        // Act
        var action = new Action(() => this.DataHelper.SaveRoleRoleDegreeLink(role, roleDegree));

        // Assert
        action.Should().Throw<UniqueViolationConstraintDALException>().WithMessage("Role-Seniority link with same:'Role,Seniority' already exists");
    }
}
