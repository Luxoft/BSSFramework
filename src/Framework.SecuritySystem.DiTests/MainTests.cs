using FluentAssertions;

using Framework.Core;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.HierarchicalExpand;
using Framework.HierarchicalExpand.DependencyInjection;
using Framework.QueryableSource;
using Framework.SecuritySystem.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public partial class MainTests
{
    private readonly BusinessUnit bu1;

    private readonly BusinessUnit bu2;

    private readonly BusinessUnit bu3;

    private readonly List<Dictionary<Type, List<Guid>>> permissions;

    private readonly Employee employee1;

    private readonly Employee employee2;

    private readonly Employee employee3;

    private readonly IServiceProvider rootServiceProvider;

    public MainTests()
    {
        this.bu1 = new BusinessUnit() { Id = Guid.NewGuid() };
        this.bu2 = new BusinessUnit() { Id = Guid.NewGuid(), Parent = this.bu1 };
        this.bu3 = new BusinessUnit() { Id = Guid.NewGuid() };

        this.permissions = new List<Dictionary<Type, List<Guid>>> { new() { { typeof(BusinessUnit), new[] { this.bu1.Id }.ToList() } } };


        this.employee1 = new Employee() { Id = Guid.NewGuid(), BusinessUnit = this.bu1 };
        this.employee2 = new Employee() { Id = Guid.NewGuid(), BusinessUnit = this.bu2 };
        this.employee3 = new Employee() { Id = Guid.NewGuid(), BusinessUnit = this.bu3 };

        this.rootServiceProvider = this.BuildRootServiceProvider();
    }

    [Fact]
    public async Task TestEmployeesSecurity_EmployeeHasAccessCorrect()
    {
        // Arrange
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var employeeDomainSecurityService = scope.ServiceProvider.GetRequiredService<IDomainSecurityService<Employee>>();
        var counterService = scope.ServiceProvider.GetRequiredService<BusinessUnitAncestorLinkSourceExecuteCounter>();
        var securityProvider = employeeDomainSecurityService.GetSecurityProvider(SecurityRule.View);

        // Act
        var result1 = securityProvider.HasAccess(this.employee1);
        var result2 = securityProvider.HasAccess(this.employee2);
        var result3 = securityProvider.HasAccess(this.employee3);

        // Assert
        result1.Should().BeTrue();
        result2.Should().BeTrue();
        result3.Should().BeFalse();

        counterService.Count.Should().Be(1);
    }

    [Fact]
    public async Task CheckEmployeeWithoutSecurity_ExceptionRaised()
    {
        // Arrange
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var employeeDomainSecurityService = scope.ServiceProvider.GetRequiredService<IDomainSecurityService<Employee>>();
        var accessDeniedExceptionService = scope.ServiceProvider.GetRequiredService<IAccessDeniedExceptionService>();

        var securityProvider = employeeDomainSecurityService.GetSecurityProvider(SecurityRule.View);


        // Act
        var checkAccessAction = () => securityProvider.CheckAccess(this.employee3, accessDeniedExceptionService);

        // Assert
        checkAccessAction.Should().Throw<AccessDeniedException>();
    }


    private IServiceProvider BuildRootServiceProvider()
    {
        return new ServiceCollection()

               .RegisterHierarchicalObjectExpander()
               .AddScoped(this.BuildQueryableSource)

               .AddSecuritySystem(
                   settings =>

                       settings
                           .AddPermissionSystem<ExamplePermissionSystem>()

                           .AddDomainSecurityServices(
                                   rb =>
                                       rb.Add<Employee>(
                                           b => b.SetView(ExampleSecurityOperation.EmployeeView)
                                                 .SetEdit(ExampleSecurityOperation.EmployeeEdit)
                                                 .SetPath(SecurityPath<Employee>.Create(v => v.BusinessUnit))))

                               .AddSecurityContext<BusinessUnit>(Guid.NewGuid())
                               .AddSecurityContext<Location>(Guid.NewGuid())

                               .AddSecurityRole(
                                   ExampleSecurityRole.TestRole,
                                   new SecurityRoleInfo(Guid.NewGuid())
                                   {
                                       Children = [ExampleSecurityRole.TestRole2],
                                       Operations = [ExampleSecurityOperation.EmployeeView, ExampleSecurityOperation.EmployeeEdit]
                                   })

                               .AddSecurityRole(
                                   ExampleSecurityRole.TestRole2,
                                   new SecurityRoleInfo(Guid.NewGuid()) { Children = [ExampleSecurityRole.TestRole3] })

                               .AddSecurityRole(
                                   ExampleSecurityRole.TestRole3,
                                   new SecurityRoleInfo(Guid.NewGuid()))

                               .AddSecurityRole(
                                   ExampleSecurityRole.TestRole4,
                                   new SecurityRoleInfo(Guid.NewGuid()) { Operations = [ExampleSecurityOperation.BusinessUnitView] })

                               .AddSecurityRole(SecurityRole.Administrator, new SecurityRoleInfo(Guid.NewGuid()))

                               .AddSecurityOperation(
                                   ExampleSecurityOperation.BusinessUnitView,
                                   new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.None })

                   )

               .AddRelativeDomainPath((Employee employee) => employee)
               .AddSingleton(typeof(TestCheckboxConditionFactory<>))

               .AddScoped(_ => LazyInterfaceImplementHelper.CreateNotImplemented<IUserAuthenticationService>())

               .AddSingleton(new SecurityPathRestrictionServiceSettings { ValidateSecurityPath = true })

               .AddScoped<BusinessUnitAncestorLinkSourceExecuteCounter>()
               .AddSingleton(new ExamplePermissionSystemData(this.permissions))

               .ValidateDuplicateDeclaration()
               .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }

    private IQueryableSource BuildQueryableSource(IServiceProvider serviceProvider)
    {
        var queryableSource = Substitute.For<IQueryableSource>();

        queryableSource.GetQueryable<BusinessUnitAncestorLink>()
                       .Returns(this.GetBusinessUnitAncestorLinkSource(serviceProvider).AsQueryable());
        queryableSource.GetQueryable<Employee>().Returns(new[] { this.employee1, this.employee2, this.employee3 }.AsQueryable());

        return queryableSource;
    }

    private IEnumerable<BusinessUnitAncestorLink> GetBusinessUnitAncestorLinkSource(IServiceProvider serviceProvider)
    {
        var counter = serviceProvider.GetRequiredService<BusinessUnitAncestorLinkSourceExecuteCounter>();
        counter.Count++;

        yield return new BusinessUnitAncestorLink { Ancestor = this.bu1, Child = this.bu1 };
        yield return new BusinessUnitAncestorLink { Ancestor = this.bu2, Child = this.bu2 };
        yield return new BusinessUnitAncestorLink { Ancestor = this.bu3, Child = this.bu3 };

        yield return new BusinessUnitAncestorLink { Ancestor = this.bu1, Child = this.bu2 };
    }

    private class BusinessUnitAncestorLinkSourceExecuteCounter
    {
        public int Count { get; set; }
    }
}
