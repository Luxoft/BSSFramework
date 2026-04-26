using Anch.DependencyInjection;

using Framework.Database.EntityFramework.DependencyInjection;
using Framework.Infrastructure.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using SampleSystem.WebApiCore.Domain;

using Anch.SecuritySystem;

namespace SampleSystem.WebApiCore.DependencyInjection;

public static class SampleSystemGeneralDependencyInjectionExtensions
{
    public static IServiceCollection AddGeneralDependencyInjection(this IServiceCollection services, IConfiguration configuration) =>
        services

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
                    .AddEntityFramework(_ => { });
            })
            .AddScopedFrom<DbContext, AppDbContext>()
            .AddDbContext<AppDbContext>(o =>
                                            o.UseSqlServer(
                                                "Data Source=.;Initial Catalog=SampleSystem;Integrated Security=True;TrustServerCertificate=True"))
            .AddHttpContextAccessor()
            .AddLogging();
}
