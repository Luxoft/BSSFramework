using Framework.DomainDriven.EntityFramework;
using Framework.DomainDriven.Setup;

using Microsoft.EntityFrameworkCore;

using SampleSystem.Domain;
using SampleSystem.Security;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection RegisterGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        return services

               .AddBssFramework(
                   rootSettings =>
                   {
                       rootSettings
                           .AddSecuritySystem(
                               securitySettings =>
                                   securitySettings
                                       .AddSecurityContexts()
                                       .AddDomainSecurityServices()
                                       .AddSecurityRoles()
                                       .SetUserSource<Employee>(
                                           employee => employee.Id,
                                           employee => employee.Login,
                                           employee => employee.Active)
                                       .AddVirtualPermissions()
                                       .SetSecurityAdministratorRule(SampleSystemSecurityRole.SeManager))
                           .AddEntityFramework(s => {});
                   })
               .AddScopedFrom<DbContext, AppDbContext>()
               .AddDbContext<AppDbContext>(
                   o =>
                       o.UseSqlServer(
                           "Data Source=.;Initial Catalog=SampleSystem;Integrated Security=True;TrustServerCertificate=True"))
               .AddHttpContextAccessor()
               .AddLogging()
               .Replace(ServiceDescriptor.Scoped(_ => UserAuthenticationService.CreateFor("testEmployeeLogin")));
    }
}
