using Framework.Database.DALExceptions;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class UniqueGroupTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void UniqueGroup_NonUniqueEntityCreated_ErrorUsesCustomName()
    {
        // Arrange
        var role = this.DataHelper.SaveEmployeeRole();
        var roleDegree = this.DataHelper.SaveEmployeeRoleDegree();

        this.DataHelper.SaveRoleRoleDegreeLink(role, roleDegree);

        // Act
        var action = new Action(() => this.DataHelper.SaveRoleRoleDegreeLink(role, roleDegree));

        // Assert
        Assert.Equal("Role-Seniority link with same:'Role,Seniority' already exists", Assert.Throws<UniqueViolationConstraintDALException>(action).Message);
    }
}
