using FluentAssertions;

using Framework.HierarchicalExpand;
using Framework.QueryableSource;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.Rules.Builders;
using V1 = Framework.SecuritySystem.Rules.Builders.MaterializedPermissions;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Xunit;

namespace Framework.SecuritySystem.DiTests;

public class MainTests
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

        this.permissions = new List<Dictionary<Type, List<Guid>>>
                           {
                                   new()
                                   {
                                           { typeof(BusinessUnit), new [] { this.bu1.Id }.ToList() }
                                   }
                           };


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
        var securityProvider = employeeDomainSecurityService.GetSecurityProvider(BLLSecurityMode.View);

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

        var securityProvider = employeeDomainSecurityService.GetSecurityProvider(BLLSecurityMode.View);


        // Act
        var checkAccessAction = () => securityProvider.CheckAccess(this.employee3, accessDeniedExceptionService);

        // Assert
        checkAccessAction.Should().Throw<AccessDeniedException>();
    }


    private IServiceProvider BuildRootServiceProvider()
    {
        return new ServiceCollection()

               .AddScoped<BusinessUnitAncestorLinkSourceExecuteCounter>()

               .AddScoped(this.BuildQueryableSource)
               .AddScoped<IPrincipalPermissionSource<Guid>>(_ => new ExamplePrincipalPermissionSource(this.permissions))

               .AddSingleton<IAccessDeniedExceptionService, AccessDeniedExceptionService<Guid>>()
               .AddSingleton<IDisabledSecurityProviderSource, DisabledSecurityProviderSource>()

               .AddScoped<ISecurityExpressionBuilderFactory, V1.SecurityExpressionBuilderFactory<Guid>>()
               .AddScoped<IAuthorizationSystem<Guid>, ExampleAuthorizationSystem>()
               .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<Guid>>()

               .RegisterDomainSecurityServices<Guid>(
                   rb =>
                       rb.Add<Employee>(
                           b => b.SetView(ExampleSecurityOperation.EmployeeView)
                                 .SetEdit(ExampleSecurityOperation.EmployeeEdit)
                                 .SetPath(SecurityPath<Employee>.Create(v => v.BusinessUnit))))

               .AddSingleton<ISecurityOperationResolver, SecurityOperationResolver>()

               .AddSingleton<IRealTypeResolver, IdentityRealTypeResolver>()

               .AddSingleton<ISecurityContextInfoService, SecurityContextInfoService>()
               .RegisterSecurityContextInfoService<Guid>(b => b.Add<BusinessUnit>(Guid.NewGuid()))

               .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }

    private IQueryableSource BuildQueryableSource(IServiceProvider serviceProvider)
    {
        var queryableSource = Substitute.For<IQueryableSource>();

        queryableSource.GetQueryable<BusinessUnitAncestorLink>().Returns(this.GetBusinessUnitAncestorLinkSource(serviceProvider).AsQueryable());
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
