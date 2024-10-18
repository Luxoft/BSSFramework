using Framework.Core;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.HierarchicalExpand;
using Framework.HierarchicalExpand.DependencyInjection;
using Framework.QueryableSource;
using Framework.SecuritySystem.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace Framework.SecuritySystem.DiTests;

public abstract class TestBase
{
    protected readonly List<TestPermission> permissions = [];

    protected readonly IServiceProvider rootServiceProvider;

    public TestBase()
    {
        this.rootServiceProvider = LazyInterfaceImplementHelper.CreateProxy(() => this.BuildRootServiceProvider(new ServiceCollection()));
    }

    protected virtual IServiceProvider BuildRootServiceProvider(IServiceCollection serviceCollection)
    {
        return serviceCollection

               .RegisterHierarchicalObjectExpander()
               .AddScoped(this.BuildQueryableSource)

               .AddSecuritySystem(
                   settings =>

                       settings
                           .AddPermissionSystem<ExamplePermissionSystemFactory>()

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

                           .AddSecurityRole(
                               ExampleSecurityRole.TestKeyedRole,
                               new SecurityRoleInfo(Guid.NewGuid()) { Restriction = SecurityPathRestriction.Create<Location>().Add<BusinessUnit>(key: "testKey") })

                           .AddSecurityRole(SecurityRole.Administrator, new SecurityRoleInfo(Guid.NewGuid()))

                           .AddSecurityOperation(
                               ExampleSecurityOperation.BusinessUnitView,
                               new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.None })

                   )

               .AddRelativeDomainPath((Employee employee) => employee)
               .AddSingleton(typeof(TestCheckboxConditionFactory<>))

               .AddNotImplemented<IUserAuthenticationService>()

               .AddSingleton(new TestPermissionData(this.permissions))

               .ValidateDuplicateDeclaration()
               .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
    }

    protected virtual IQueryableSource BuildQueryableSource(IServiceProvider serviceProvider)
    {
        return Substitute.For<IQueryableSource>();
    }
}
