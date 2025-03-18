using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;
using SampleSystem.Domain.EnversBug1676;
using SampleSystem.Domain.ManualProjections;
using SampleSystem.Domain.Projections;
using SampleSystem.Security.Metadata;
using SampleSystem.Security.Services;

namespace SampleSystem.Security;

public static class SampleSystemDomainSecurityServiceExtensions
{
    public static ISecuritySystemSettings AddDomainSecurityServices(this ISecuritySystemSettings settings)
    {
        return settings.RegisterMainDomainSecurityServices()
                       .RegisterDisabledDomainSecurityServices()
                       .RegisterLegacyProjectionDomainSecurityServices()

                       .AddExtensions(new ProjectionDomainSecurityBssFrameworkExtension(typeof(TestBusinessUnit).Assembly))
                       .AddExtensions(new ProjectionDomainSecurityBssFrameworkExtension(typeof(TestManualEmployeeProjection).Assembly));
    }

    private static ISecuritySystemSettings RegisterMainDomainSecurityServices(this ISecuritySystemSettings settings)
    {
        return settings.AddDomainSecurityServices(

            rb =>

                rb.AddMetadata<SampleSystemEmployeeDomainSecurityServiceMetadata>()
                  .AddMetadata<SampleSystemEmployeeCellPhoneDomainSecurityServiceMetadata>()

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
                      b => b.SetView(SampleSystemSecurityRole.TestPerformance)
                            .SetPath(
                                SecurityPath<TestPerformanceObject>.Create(v => v.Location, true)
                                                                   .And(v => v.Employee, true)
                                                                   .And(v => v.BusinessUnit, true)
                                                                   .And(v => v.ManagementUnit, true)))

                  .Add<TestPlainAuthObject>(
                      b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                            .SetPath(
                                SecurityPath<TestPlainAuthObject>.Create(v => v.Location)
                                                                 .And(SecurityPath<TestPlainAuthObject>.CreateNested(v => v.Items,
                                                                      SecurityPath<TestItemAuthObject>.Create(i => i.BusinessUnit).And(i => i.ManagementUnit)))))

                  .Add<AuthPerformanceObject>(
                      b => b.SetView(SampleSystemSecurityRole.TestPerformance)
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

                  .Add<TestRestrictionObject>(new[] { SampleSystemSecurityRole.RestrictionRole}, SecurityPath<TestRestrictionObject>.Create(v => v.BusinessUnit))

                  .Add<TestCustomContextSecurityObj>(b => b.SetCustomService<SampleSystemTestCustomContextSecurityObjSecurityService>())

                  .Add<TestSecurityObjItem>(b => b.SetDependency(v => v.FirstMaster))
                  .Add<TestSecuritySubObjItem>(b => b.SetDependency(v => v.InnerMaster))
                  .Add<TestSecuritySubObjItem2>(b => b.SetDependency(v => v.RootSecurityObj))
                  .Add<TestSecuritySubObjItem3>(b => b.SetDependency(v => v.InnerMaster.FirstMaster))
            );
    }

    private static ISecuritySystemSettings RegisterDisabledDomainSecurityServices(this ISecuritySystemSettings settings)
    {
        return settings.AddDomainSecurityServices(

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

    private static ISecuritySystemSettings RegisterLegacyProjectionDomainSecurityServices(this ISecuritySystemSettings settings)
    {
        return settings.AddDomainSecurityServices(

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
