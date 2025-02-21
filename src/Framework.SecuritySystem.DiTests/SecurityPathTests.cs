using FluentAssertions;

using Framework.Core;
using Framework.QueryableSource;
using Framework.SecuritySystem.Services;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public class SecurityPathTests : TestBase
{
    private readonly BusinessUnit bu1;

    public SecurityPathTests()
    {
        this.bu1 = new BusinessUnit() { Id = Guid.NewGuid() };

        this.permissions.Override(
        [
            new TestPermission(
                ExampleSecurityRole.TestKeyedRole,
                new Dictionary<Type, IReadOnlyList<Guid>> { { typeof(BusinessUnit), [this.bu1.Id] } })
        ]);
    }

    [Fact]
    public void TryApplyRestriction_RestrictionApplied()
    {
        //Arrange
        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var buExpr = ExpressionHelper.Create((Employee employee) => employee.BusinessUnit);
        var locationExpr = ExpressionHelper.Create((Employee employee) => employee.Location);
        var conditionExpr = ExpressionHelper.Create((Employee employee) => employee.TestCheckbox);

        var testSecurityPath = SecurityPath<Employee>.Create(buExpr).And(locationExpr);

        var restriction = SecurityPathRestriction.Create<BusinessUnit>().AddConditionFactory(typeof(TestCheckboxConditionFactory<>));

        var expectedNewSecurityPath = SecurityPath<Employee>.Create(buExpr).And(conditionExpr);

        //Act
        var newSecurityPath = service.ApplyRestriction(testSecurityPath, restriction);

        //Assert
        newSecurityPath.Should().Be(expectedNewSecurityPath);
    }

    [Fact]
    public void TryApplyOverflowRestriction_ResultPathIsEmpty()
    {
        //Arrange
        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var testSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit);

        var restriction = SecurityPathRestriction.Create<Location>();

        //Act
        var result = service.ApplyRestriction(testSecurityPath, restriction);

        //Assert
        result.Should().Be(SecurityPath<Employee>.Empty);
    }

    [Fact]
    public void TryApplyKeyedRestriction_SecurityPathCorrect()
    {
        //Arrange
        var key = nameof(Employee.AltBusinessUnit);

        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var baseSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit);
        var altSecurityPath = SecurityPath<Employee>.Create(employee => employee.AltBusinessUnit, key: key);
        var testSecurityPath = baseSecurityPath.And(altSecurityPath);

        var restriction = SecurityPathRestriction.Create<Location>().Add<BusinessUnit>(key: key);

        //Act
        var result = service.ApplyRestriction(testSecurityPath, restriction);

        //Assert
        result.Should().Be(altSecurityPath);
    }

    [Fact]
    public void EmptySecurityPathRestriction_SecurityPathNotModified()
    {
        //Arrange
        var key = nameof(Employee.AltBusinessUnit);

        var service = this.rootServiceProvider.GetRequiredService<ISecurityPathRestrictionService>();

        var baseSecurityPath = SecurityPath<Employee>.Create(employee => employee.BusinessUnit);
        var altSecurityPath = SecurityPath<Employee>.Create(employee => employee.AltBusinessUnit, key: key);

        var testSecurityPath = baseSecurityPath.And(altSecurityPath);

        var restriction = SecurityPathRestriction.Default;

        //Act
        var result = service.ApplyRestriction(testSecurityPath, restriction);

        //Assert
        result.Should().Be(testSecurityPath);
    }

    [Fact]
    public async Task KeyedSecurityPath_WithStrictly_EmployeeExcepted()
    {
        // Arrange
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var testSecurityPath = SecurityPath<Employee>
                               .Create(employee => employee.Location)
                               .And(employee => employee.BusinessUnit, SingleSecurityMode.Strictly, key: "testKey");

        var securityProvider = scope.ServiceProvider.GetRequiredService<IDomainSecurityProviderFactory<Employee>>()
                                    .Create(ExampleSecurityRole.TestKeyedRole, testSecurityPath);

        var testEmployee1 = new Employee { BusinessUnit = this.bu1 };
        var testEmployee2 = new Employee();

        //Act
        var result1 = securityProvider.HasAccess(testEmployee1);
        var result2 = securityProvider.HasAccess(testEmployee2);

        //Assert
        result1.Should().BeTrue();
        result2.Should().BeFalse();
    }

    [Fact]
    public async Task KeyedSecurityPath_WithoutStrictly_EmployeeIncluded()
    {
        // Arrange
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var testSecurityPath = SecurityPath<Employee>
                               .Create(employee => employee.Location)
                               .And(employee => employee.BusinessUnit, SingleSecurityMode.AllowNull, key: "testKey");

        var securityProvider = scope.ServiceProvider.GetRequiredService<IDomainSecurityProviderFactory<Employee>>()
                                    .Create(ExampleSecurityRole.TestKeyedRole, testSecurityPath);

        var testEmployee1 = new Employee { BusinessUnit = this.bu1 };
        var testEmployee2 = new Employee();

        //Act
        var result1 = securityProvider.HasAccess(testEmployee1);
        var result2 = securityProvider.HasAccess(testEmployee2);

        //Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
    }

    protected override IQueryableSource BuildQueryableSource(IServiceProvider serviceProvider)
    {
        var queryableSource = Substitute.For<IQueryableSource>();

        queryableSource.GetQueryable<BusinessUnitAncestorLink>()
                       .Returns(this.GetBusinessUnitAncestorLinkSource().AsQueryable());

        return queryableSource;
    }
    private IEnumerable<BusinessUnitAncestorLink> GetBusinessUnitAncestorLinkSource()
    {
        yield return new BusinessUnitAncestorLink { Ancestor = this.bu1, Child = this.bu1 };
    }
}
