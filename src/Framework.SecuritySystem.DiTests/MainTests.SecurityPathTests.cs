using FluentAssertions;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public partial class MainTests
{
    [Fact]
    public void TryApplyRestriction_RestrictionApplied()
    {
        //Arrange
        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var baseSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit)
                                                 .And(employee => employee.Location);

        var restriction = SecurityPathRestriction.Create<BusinessUnit>().Add((Employee employee) => employee.TestCheckbox);

        //Act
        var newSecurityPath = service.ApplyRestriction(baseSecurityPath, restriction);

        //Assert
        newSecurityPath.GetUsedTypes().Should().BeEquivalentTo(restriction.SecurityContexts);
    }
}
