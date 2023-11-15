using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain.EnversBug1676;
using SampleSystem.Domain.ManualProjections;
using SampleSystem.Security.Metadata;
using SampleSystem.Security.Services;
using Framework.Authorization.SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityServiceExtensions
{
    public static IServiceCollection RegisterGeneralSecurityServices(this IServiceCollection services)
    {
        return services.RegisterSecurityContexts()
                       .RegisterSecurityOperations()
                       .RegisterSecurityRoles()
                       .RegisterDomainSecurityServices();
    }

    private static IServiceCollection RegisterSecurityContexts(this IServiceCollection services)
    {
        return services.RegisterSecurityContextInfoService<Guid>(
            b => b.Add<BusinessUnit>(new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"))
                  .Add<Location>(new Guid("4641395B-9079-448E-9CB8-A083015235A3"))
                  .Add<ManagementUnit>(new Guid("77E78AEF-9512-46E0-A33D-AAE58DC7E18C"))
                  .Add<Employee>(new Guid("B3F2536E-27C4-4B91-AE0B-0EE2FFD4465F"), displayFunc: employee => employee.Login));
    }

    private static IServiceCollection RegisterSecurityOperations(this IServiceCollection services)
    {
        return services.AddSingleton(new SecurityOperationTypeInfo(typeof(SampleSystemSecurityOperation)));
    }

    private static IServiceCollection RegisterSecurityRoles(this IServiceCollection services)
    {
        return services.AddSingleton(new SecurityRoleTypeInfo(typeof(SampleSystemSecurityRole)));
    }

    private static IServiceCollection RegisterDomainSecurityServices(this IServiceCollection services)
    {
        return services.RegisterMainDomainSecurityServices()
                       .RegisterDisabledDomainSecurityServices()
                       .RegisterLegacyProjectionDomainSecurityServices()
                       .RegisterProjectionDomainSecurityServices(typeof(TestBusinessUnit).Assembly)
                       .RegisterProjectionDomainSecurityServices(typeof(TestManualEmployeeProjection).Assembly);
    }

    private static IServiceCollection RegisterMainDomainSecurityServices(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(

            rb =>

                rb.AddMetadata<SampleSystemEmployeeDomainSecurityServiceMetadata>()

                  .Add(SampleSystemSecurityOperation.BusinessUnitView,
                       SampleSystemSecurityOperation.BusinessUnitEdit,
                       SecurityPath<BusinessUnit>.Create(fbu => fbu))

                  .Add<BusinessUnitType>(
                      SampleSystemSecurityOperation.BusinessUnitTypeView,
                      SampleSystemSecurityOperation.BusinessUnitTypeEdit)

                  .Add(
                      SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkView,
                      SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkEdit,
                      SecurityPath<BusinessUnitManagerCommissionLink>.Create(v => v.BusinessUnit))

                  .Add(
                      SampleSystemSecurityOperation.BusinessUnitHrDepartmentView,
                      SampleSystemSecurityOperation.BusinessUnitHrDepartmentEdit,
                      SecurityPath<BusinessUnitHrDepartment>.Create(v => v.BusinessUnit).And(v => v.HRDepartment.Location))

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

                  .Add<EmployeeCellPhone>(b => b.SetDependency(v => v.Employee))

                  .Add<ManagementUnitFluentMapping>(
                      b => b.SetView(SampleSystemSecurityOperation.ManagementUnitView)
                            .SetEdit(SampleSystemSecurityOperation.ManagementUnitEdit))

                  .Add<Example1>(
                      b => b.SetView(SampleSystemSecurityOperation.LocationView)
                            .SetEdit(SampleSystemSecurityOperation.LocationEdit))

                  .Add<TestCustomContextSecurityObj>(b => b.SetCustomService<SampleSystemTestCustomContextSecurityObjSecurityService>())

                  .Add<TestSecurityObjItem>(b => b.SetDependency(v => v.FirstMaster))
                  .Add<TestSecuritySubObjItem>(b => b.SetDependency(v => v.InnerMaster))
                  .Add<TestSecuritySubObjItem2>(b => b.SetDependency(v => v.RootSecurityObj))
                  .Add<TestSecuritySubObjItem3>(b => b.SetDependency(v => v.InnerMaster.FirstMaster))
            );
    }

    private static IServiceCollection RegisterDisabledDomainSecurityServices(this IServiceCollection services)
    {
        return services.RegisterDomainSecurityServices<Guid>(

            rb =>

                rb.AddViewDisabled<EmployeeInformation>()
                  .AddViewDisabled<EmployeeRegistrationType>()
                  .AddViewDisabled<IMRequest>()
                  .AddViewDisabled<Information>()
                  .AddViewDisabled<Location1676>()
                  .AddViewDisabled<WorkingCalendar1676>()
                  .AddViewDisabled<Principal>()
                  .AddViewDisabled<SqlParserTestObj>()
                  .AddViewDisabled<SqlParserTestObjContainer>()

                  .AddFullDisabled<TestImmutableObj>()
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
