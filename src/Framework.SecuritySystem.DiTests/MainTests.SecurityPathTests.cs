﻿using FluentAssertions;

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

        var restriction = SecurityPathRestriction.Create<BusinessUnit>().AddConditionFactory(typeof(TestCheckboxConditionFactory<>));

        var expectedNewSecurityPath = SecurityPath<Employee>.Create(buExpr).And(conditionExpr);

        //Act
        var newSecurityPath = service.ApplyRestriction(baseSecurityPath, restriction);

        //Assert
        newSecurityPath.Should().Be(expectedNewSecurityPath);
    }

    [Fact]
    public void TryApplyOverflowRestriction_ResultPathIsEmpty()
    {
        //Arrange
        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var baseSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit);

        var restriction = SecurityPathRestriction.Create<Location>();

        //Act
        var result = service.ApplyRestriction(baseSecurityPath, restriction);

        //Assert
        result.Should().Be(SecurityPath<Employee>.Empty);
    }

    [Fact]
    public void TryApplyKeyedRestriction_SecurityPathCorrect()
    {
        //Arrange
        var key = "Alt";

        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var altSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit, key: key);
        var baseSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit).And(altSecurityPath);

        var restriction = SecurityPathRestriction.Create<Location>().Add<BusinessUnit>(key: key);

        //Act
        var result = service.ApplyRestriction(baseSecurityPath, restriction);

        //Assert
        result.Should().Be(altSecurityPath);
    }
}
