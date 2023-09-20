using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

using Framework.SecuritySystem.DependencyInjection;
using SampleSystem.Domain.EnversBug1676;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemDomainSecurityServiceExtensions
{
    public static IServiceCollection RegisterDomainSecurityServices(this IServiceCollection services)
    {
        return services.RegisterMainDomainSecurityServices()
                       .RegisterDisabledDomainSecurityServices()
                       .RegisterLegacyProjectionDomainSecurityServices()
                       .RegisterProjectionDomainSecurityServices(typeof(TestBusinessUnit).Assembly);
    }

    private static IServiceCollection RegisterMainDomainSecurityServices(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(

            rb =>

                rb.Add<Employee>(
                      b =>
                          b.SetView(SampleSystemSecurityOperation.EmployeeView)
                           .SetEdit(SampleSystemSecurityOperation.EmployeeEdit)
                           .SetPath(
                               SecurityPath<Employee>.Create(employee => employee).And(employee => employee.CoreBusinessUnit)
                                                     .And(employee => employee.Location))
                           .SetCustomService<SampleSystemEmployeeSecurityService>())

                  .Add<BusinessUnit>(
                      b => b.SetView(SampleSystemSecurityOperation.BusinessUnitView)
                            .SetEdit(SampleSystemSecurityOperation.BusinessUnitEdit)
                            .SetPath(SecurityPath<BusinessUnit>.Create(fbu => fbu)))

                  .Add<BusinessUnitType>(
                      b => b.SetView(SampleSystemSecurityOperation.BusinessUnitTypeView)
                            .SetEdit(SampleSystemSecurityOperation.BusinessUnitTypeEdit))

                  .Add<BusinessUnitManagerCommissionLink>(
                      b => b.SetView(SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkView)
                            .SetEdit(SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkEdit)
                            .SetPath(SecurityPath<BusinessUnitManagerCommissionLink>.Create(v => v.BusinessUnit)))

                  .Add<BusinessUnitHrDepartment>(
                      b => b.SetView(SampleSystemSecurityOperation.BusinessUnitHrDepartmentView)
                            .SetEdit(SampleSystemSecurityOperation.BusinessUnitHrDepartmentEdit)
                            .SetPath(SecurityPath<BusinessUnitHrDepartment>.Create(v => v.BusinessUnit).And(v => v.HRDepartment.Location)))

                  .Add<ManagementUnit>(
                      b => b.SetView(SampleSystemSecurityOperation.ManagementUnitView)
                            .SetEdit(SampleSystemSecurityOperation.ManagementUnitEdit)
                            .SetPath(SecurityPath<ManagementUnit>.Create(mbu => mbu)))

                  .Add<ManagementUnitAndBusinessUnitLink>(
                      b => b.SetView(SampleSystemSecurityOperation.ManagementUnitAndBusinessUnitLinkView)
                            .SetEdit(SampleSystemSecurityOperation.ManagementUnitAndBusinessUnitLinkEdit)
                            .SetPath(
                                SecurityPath<ManagementUnitAndBusinessUnitLink>.Create(v => v.BusinessUnit).And(v => v.ManagementUnit)))

                  .Add<ManagementUnitAndHRDepartmentLink>(
                      b => b.SetView(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkView)
                            .SetEdit(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkEdit)
                            .SetPath(
                                SecurityPath<ManagementUnitAndHRDepartmentLink>.Create(v => v.ManagementUnit)
                                                                               .And(v => v.HRDepartment.Location)))

                  .Add<EmployeeSpecialization>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeSpecializationView))

                  .Add<EmployeeRole>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeRoleView))

                  .Add<EmployeeRoleDegree>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeRoleDegreeView))

                  .Add<HRDepartment>(
                      b => b.SetView(SampleSystemSecurityOperation.HRDepartmentView)
                            .SetEdit(SampleSystemSecurityOperation.HRDepartmentEdit))

                  .Add<Location>(
                      b => b.SetView(SampleSystemSecurityOperation.LocationView)
                            .SetEdit(SampleSystemSecurityOperation.LocationEdit))

                  .Add<Country>(
                      b => b.SetView(SampleSystemSecurityOperation.CountryView)
                            .SetEdit(SampleSystemSecurityOperation.CountryEdit))

                  .Add<CompanyLegalEntity>(
                      b => b.SetView(SampleSystemSecurityOperation.CompanyLegalEntityView)
                            .SetEdit(SampleSystemSecurityOperation.CompanyLegalEntityEdit))

                  .Add<EmployeePosition>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeePositionView)
                            .SetEdit(SampleSystemSecurityOperation.EmployeePositionEdit)
                            .SetPath(SecurityPath<EmployeePosition>.Create(position => position.Location)))

                  .Add<EmployeePersonalCellPhone>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView)
                            .SetEdit(SampleSystemSecurityOperation.EmployeePersonalCellPhoneEdit))

                  .Add<TestRootSecurityObj>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                            .SetPath(SecurityPath<TestRootSecurityObj>.Create(v => v.BusinessUnit).And(v => v.Location)))

                  .Add<TestPerformanceObject>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                            .SetPath(
                                SecurityPath<TestPerformanceObject>.Create(v => v.Location, SingleSecurityMode.Strictly)
                                                                   .And(v => v.Employee, SingleSecurityMode.Strictly)
                                                                   .And(v => v.BusinessUnit, SingleSecurityMode.Strictly)
                                                                   .And(v => v.ManagementUnit, SingleSecurityMode.Strictly)))

                  .Add<TestPlainAuthObject>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                            .SetPath(
                                SecurityPath<TestPlainAuthObject>.Create(v => v.Location)
                                                                 .And(
                                                                     v => v.Items.Select(item => item.BusinessUnit),
                                                                     ManySecurityPathMode.All)
                                                                 .And(
                                                                     v => v.Items.Select(item => item.ManagementUnit),
                                                                     ManySecurityPathMode.All)))

                  .Add<AuthPerformanceObject>(
                      b => b.SetView(SampleSystemSecurityOperation.BusinessUnitView)
                            .SetPath(
                                SecurityPath<AuthPerformanceObject>.Create(v => v.BusinessUnit)
                                                                   .And(v => v.ManagementUnit)
                                                                   .And(v => v.Location)
                                                                   .And(v => v.Employee)))

                  .Add<EmployeePhoto>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                            .SetPath(SecurityPath<EmployeePhoto>.Create(employeePhoto => employeePhoto.Employee.CoreBusinessUnit)))

                  .Add<ManagementUnitFluentMapping>(
                      b => b.SetView(SampleSystemSecurityOperation.ManagementUnitView)
                            .SetEdit(SampleSystemSecurityOperation.ManagementUnitEdit))

                  .Add<Example1>(
                      b => b.SetView(SampleSystemSecurityOperation.LocationView)
                            .SetEdit(SampleSystemSecurityOperation.LocationEdit))

                  .Add<TestCustomContextSecurityObj>(b => b.SetCustomService<SampleSystemTestCustomContextSecurityObjSecurityService>())
            );
    }

    private static IServiceCollection RegisterDisabledDomainSecurityServices(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(

            rb =>

                rb.Add<EmployeeInformation>(b => b.SetView(SampleSystemSecurityOperation.Disabled))
                  .Add<EmployeeRegistrationType>(b => b.SetView(SampleSystemSecurityOperation.Disabled))
                  .Add<IMRequest>(b => b.SetView(SampleSystemSecurityOperation.Disabled))
                  .Add<Information>(b => b.SetView(SampleSystemSecurityOperation.Disabled))
                  .Add<Location1676>(b => b.SetView(SampleSystemSecurityOperation.Disabled))
                  .Add<WorkingCalendar1676>(b => b.SetView(SampleSystemSecurityOperation.Disabled))
                  .Add<Principal>(
                      b => b.SetView(SampleSystemSecurityOperation.Disabled)
                            .SetEdit(SampleSystemSecurityOperation.Disabled))
                  .Add<SqlParserTestObj>(
                      b => b.SetView(SampleSystemSecurityOperation.Disabled)
                            .SetEdit(SampleSystemSecurityOperation.Disabled))
                  .Add<SqlParserTestObjContainer>(
                      b => b.SetView(SampleSystemSecurityOperation.Disabled)
                            .SetEdit(SampleSystemSecurityOperation.Disabled))
                  .Add<TestImmutableObj>(
                      b => b.SetView(SampleSystemSecurityOperation.Disabled)
                            .SetEdit(SampleSystemSecurityOperation.Disabled))
            );
    }

    private static IServiceCollection RegisterLegacyProjectionDomainSecurityServices(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(

            rb =>

                rb.Add<SecurityBusinessUnit>(
                      b => b.SetUntypedDependency<BusinessUnit>())

                  .Add<SecurityEmployee>(
                      b => b.SetUntypedDependency<Employee>())

                  .Add<SecurityHRDepartment>(
                      b => b.SetUntypedDependency<HRDepartment>())

                  .Add<SecurityLocation>(
                      b => b.SetUntypedDependency<Location>())

                  .Add<TestLegacyEmployee>(
                      b => b.SetUntypedDependency<Employee>())
            );
    }
}
