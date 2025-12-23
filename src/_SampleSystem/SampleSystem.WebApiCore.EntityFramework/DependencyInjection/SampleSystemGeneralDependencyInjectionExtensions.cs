using CommonFramework.DependencyInjection;

using Framework.DomainDriven.EntityFramework;
using Framework.DomainDriven.Setup;

using Microsoft.EntityFrameworkCore;

using SampleSystem.Domain;
using SampleSystem.Security;

using SecuritySystem;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection RegisterGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        return services

               .AddBssFramework(rootSettings =>
               {
                   rootSettings.RegisterDenormalizeHierarchicalDALListener = false;

                   rootSettings
                       .AddSecuritySystem(securitySettings =>
                                              securitySettings
                                                  .AddSecurityContexts()
                                                  .AddDomainSecurityServices()
                                                  .AddSecurityRoles()
                                                  .AddUserSource<Employee>(usb => usb.SetFilter(employee => employee.Active))
                                                  .AddVirtualPermissions()
                                                  .SetSecurityAdministratorRule(
                                                      DomainSecurityRule.AnyRole with { CustomCredential = new SecurityRuleCredential.AnyUserCredential() }))
                       .AddEntityFramework(s => { });
               })
               .AddScopedFrom<DbContext, AppDbContext>()
               .AddDbContext<AppDbContext>(o =>
                                               o.UseSqlServer(
                                                   "Data Source=.;Initial Catalog=SampleSystem;Integrated Security=True;TrustServerCertificate=True"))
               .AddHttpContextAccessor()
               .AddLogging();
    }
}
