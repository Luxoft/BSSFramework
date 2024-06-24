using FluentAssertions;

using Framework.Core;

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

        var buExpr = ExpressionHelper.Create((Employee employee) => employee.BusinessUnit);
        var locationExpr = ExpressionHelper.Create((Employee employee) => employee.Location);
        var conditionExpr = ExpressionHelper.Create((Employee employee) => employee.TestCheckbox);

        var baseSecurityPath = SecurityPath<Employee>.Create(buExpr).And(locationExpr);

        var restriction = SecurityPathRestriction.Create<BusinessUnit>().Add(conditionExpr);

        var expectedNewSecurityPath = SecurityPath<Employee>.Create(buExpr).And(conditionExpr);

        //Act
        var newSecurityPath = service.ApplyRestriction(baseSecurityPath, restriction);

        //Assert
        newSecurityPath.Should().Be(expectedNewSecurityPath);
    }

    [Fact]
    public void TryApplyInvalidRestriction_ExceptionRaised()
    {
        //Arrange
        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var baseSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit);

        var restriction = SecurityPathRestriction.Create<Location>();

        //Act
        var getResult = () => service.ApplyRestriction(baseSecurityPath, restriction);

        //Assert
        getResult.Should().Throw<Exception>($"Can't apply restriction. Invalid types: {nameof(Location)}");
    }
}
