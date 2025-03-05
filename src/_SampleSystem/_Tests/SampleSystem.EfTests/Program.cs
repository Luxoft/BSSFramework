using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.EntityFramework;
using Framework.DomainDriven.Setup;
using Framework.DomainDriven.VirtualPermission;
using Framework.GenericQueryable;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.UserSource;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Framework.DomainDriven.Auth;
using Framework.DomainDriven.Repository;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SampleSystem.EfTests;

internal static class Program
{
    private static async Task Main()
    {
        var rootServiceProvider = BuildRootServiceProvider();

        await using var scope = rootServiceProvider.CreateAsyncScope();

        var scopeSp = scope.ServiceProvider;

        var test = await scopeSp.GetRequiredService<IAsyncDal<BusinessUnitEmployeeRole, Guid>>()
                                .GetQueryable()
                                .Include(link => link.BusinessUnit).Include(link => link.Employee)
                                .GenericToListAsync();

        var currentUserSource = scopeSp.GetRequiredService<ICurrentUserSource<Employee>>();

        var currentUser = currentUserSource.CurrentUser;

        var secEmployeeList = await scopeSp.LoadSecurityDataAsync<Employee>();

        var secBuList = await scopeSp.LoadSecurityDataAsync<BusinessUnit>();

        return;
    }

    private static async Task<List<TDomainObject>> LoadSecurityDataAsync<TDomainObject>(this IServiceProvider scopeSp)
        where TDomainObject : class
    {
        //var dbContext = scopeSp.GetRequiredService<AppDbContext>();

        var repositoryFactory = scopeSp.GetRequiredService<IRepositoryFactory<TDomainObject>>();

        var allData = await repositoryFactory.Create(SecurityRule.Disabled).GetQueryable().GenericToListAsync();

        var secData = await repositoryFactory.Create(SecurityRule.View).GetQueryable().GenericToListAsync();

        return secData;
    }

    private static IServiceProvider BuildRootServiceProvider()
    {
        return new ServiceCollection()
               .AddBssFramework(
                   rs => rs.AddEntityFramework(_ => { })
                           .AddSecuritySystem(
                               s => s
                                    .AddVirtualPermissions()
                                    .AddSecurityContext<BusinessUnit>(new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"))

                                    .AddSecurityRole(
                                        SampleSystemSecurityRole.SeManager,
                                        new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a")))

                                    .SetUserSource<Employee>(e => e.Id, e => e.Login, e => e.Active)

                                    .AddDomainSecurityServices(
                                        rb => rb

                                              .Add<BusinessUnit>(
                                                  SampleSystemSecurityRole.SeManager,
                                                  SecurityPath<BusinessUnit>.Create(bu => bu))

                                              .Add<Employee>(
                                                  SampleSystemSecurityRole.SeManager.Or(DomainSecurityRule.CurrentUser),
                                                  SecurityPath<Employee>.Create(
                                                      employee => employee.CoreBusinessUnit,
                                                      SingleSecurityMode.Strictly)))

                               ))


               .AddScopedFrom<DbContext, AppDbContext>()
               .AddDbContext<AppDbContext>(
                   o =>
                       o.UseSqlServer(
                           "Data Source=.;Initial Catalog=SampleSystem;Integrated Security=True;TrustServerCertificate=True"))
               .AddHttpContextAccessor()

               .Replace(
                   ServiceDescriptor.Singleton<IApplicationDefaultUserAuthenticationServiceSettings>(
                       new ApplicationDefaultUserAuthenticationServiceSettings("testEmployeeLogin")))

               .ValidateDuplicateDeclaration()

               //.AddNotImplemented<IDefaultConnectionStringSource>()
               //.AddSingleton(new DefaultConnectionStringSource())
               .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
    }

    private static ISecuritySystemSettings AddVirtualPermissions(this ISecuritySystemSettings settings)
    {
        return settings.AddVirtualPermission<Employee, BusinessUnitEmployeeRole>(
            SampleSystemSecurityRole.SeManager,
            link => link.Employee,
            employee => employee.Login,
            v => v.AddRestriction(link => link.BusinessUnit)
                .AddFilter(link => link.Role == BusinessUnitEmployeeRoleType.Manager));
    }
}
