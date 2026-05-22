using Framework.Database.DALExceptions;

using SampleSystem.IntegrationTests._Environment.TestData;

namespace SampleSystem.IntegrationTests;

public class UniqueGroupTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void UniqueGroup_NonUniqueEntityCreated_ErrorUsesCustomName()
    {
        // Arrange
        var role = this.DataManager.SaveEmployeeRole();
        var roleDegree = this.DataManager.SaveEmployeeRoleDegree();

        this.DataManager.SaveRoleRoleDegreeLink(role, roleDegree);

        // Act
        var ex = Record.Exception(() => this.DataManager.SaveRoleRoleDegreeLink(role, roleDegree));

        // Assert
        var uniqueViolationException = Assert.IsType<UniqueViolationConstraintDALException>(ex);
        Assert.Equal("Role-Seniority link with same:'Role,Seniority' already exists", uniqueViolationException.Message);
    }
}
