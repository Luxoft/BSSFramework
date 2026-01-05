using Framework.DomainDriven.ServiceModel.IAD;

using SampleSystem.Domain;
using SampleSystem.Domain.EnversBug1676;
using SampleSystem.Domain.ManualProjections;
using SampleSystem.Domain.Projections;
using SampleSystem.Security.Metadata;
using SampleSystem.Security.Services;

using SecuritySystem;
using SecuritySystem.DependencyInjection;

namespace SampleSystem.Security;

public static class SampleSystemDomainSecurityServiceExtensions
{
    extension(ISecuritySystemSettings settings)
    {
        public ISecuritySystemSettings AddDomainSecurityServices()
        {
            return settings.RegisterMainDomainSecurityServices()
                           .RegisterDisabledDomainSecurityServices()
                           .RegisterLegacyProjectionDomainSecurityServices()

                           .AddExtensions(new ProjectionDomainSecurityBssFrameworkExtension(typeof(TestBusinessUnit).Assembly))
                           .AddExtensions(new ProjectionDomainSecurityBssFrameworkExtension(typeof(TestManualEmployeeProjection).Assembly));
        }

        private ISecuritySystemSettings RegisterMainDomainSecurityServices()
        {
            return settings
                   .AddDomainSecurityMetadata<SampleSystemEmployeeDomainSecurityServiceMetadata>()
                   .AddDomainSecurityMetadata<SampleSystemEmployeeCellPhoneDomainSecurityServiceMetadata>()

                   .AddDomainSecurity(
                       SampleSystemSecurityOperation.BusinessUnitView,
                       SampleSystemSecurityOperation.BusinessUnitEdit,
                       SecurityPath<BusinessUnit>.Create(fbu => fbu))

                   .AddDomainSecurity<BusinessUnitType>(
                       SampleSystemSecurityOperation.BusinessUnitTypeView,
                       SampleSystemSecurityOperation.BusinessUnitTypeEdit)

                   .AddDomainSecurity(
                       SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkView,
                       SampleSystemSecurityOperation.BusinessUnitManagerCommissionLinkEdit,
                       SecurityPath<BusinessUnitManagerCommissionLink>.Create(v => v.BusinessUnit))

                   .AddDomainSecurity(
                       SampleSystemSecurityOperation.BusinessUnitHrDepartmentView,
                       SampleSystemSecurityOperation.BusinessUnitHrDepartmentEdit,
                       SecurityPath<BusinessUnitHrDepartment>.Create(v => v.BusinessUnit).And(v => v.HRDepartment.Location))

                   .AddDomainSecurity<ManagementUnit>(b => b.SetView(SampleSystemSecurityOperation.ManagementUnitView)
                                                            .SetEdit(SampleSystemSecurityOperation.ManagementUnitEdit)
                                                            .SetPath(SecurityPath<ManagementUnit>.Create(mbu => mbu)))

                   .AddDomainSecurity<ManagementUnitAndBusinessUnitLink>(b => b.SetView(SampleSystemSecurityOperation.ManagementUnitAndBusinessUnitLinkView)
                                                                               .SetEdit(SampleSystemSecurityOperation.ManagementUnitAndBusinessUnitLinkEdit)
                                                                               .SetPath(
                                                                                   SecurityPath<ManagementUnitAndBusinessUnitLink>.Create(v => v.BusinessUnit)
                                                                                       .And(v => v.ManagementUnit)))

                   .AddDomainSecurity<ManagementUnitAndHRDepartmentLink>(b => b.SetView(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkView)
                                                                               .SetEdit(SampleSystemSecurityOperation.ManagementUnitAndHRDepartmentLinkEdit)
                                                                               .SetPath(
                                                                                   SecurityPath<ManagementUnitAndHRDepartmentLink>.Create(v => v.ManagementUnit)
                                                                                       .And(v => v.HRDepartment.Location)))

                   .AddDomainSecurity<EmployeeSpecialization>(b => b.SetView(SampleSystemSecurityOperation.EmployeeSpecializationView))

                   .AddDomainSecurity<EmployeeRole>(b => b.SetView(SampleSystemSecurityOperation.EmployeeRoleView))

                   .AddDomainSecurity<EmployeeRoleDegree>(b => b.SetView(SampleSystemSecurityOperation.EmployeeRoleDegreeView))

                   .AddDomainSecurity<HRDepartment>(b => b.SetView(SampleSystemSecurityOperation.HRDepartmentView)
                                                          .SetEdit(SampleSystemSecurityOperation.HRDepartmentEdit))

                   .AddDomainSecurity<Location>(b => b.SetView(SampleSystemSecurityOperation.LocationView)
                                                      .SetEdit(SampleSystemSecurityOperation.LocationEdit))

                   .AddDomainSecurity<Country>(b => b.SetView(SampleSystemSecurityOperation.CountryView)
                                                     .SetEdit(SampleSystemSecurityOperation.CountryEdit))

                   .AddDomainSecurity<CompanyLegalEntity>(b => b.SetView(SampleSystemSecurityOperation.CompanyLegalEntityView)
                                                                .SetEdit(SampleSystemSecurityOperation.CompanyLegalEntityEdit))

                   .AddDomainSecurity<EmployeePosition>(b => b.SetView(SampleSystemSecurityOperation.EmployeePositionView)
                                                              .SetEdit(SampleSystemSecurityOperation.EmployeePositionEdit)
                                                              .SetPath(SecurityPath<EmployeePosition>.Create(position => position.Location)))

                   .AddDomainSecurity<EmployeePersonalCellPhone>(b => b.SetView(SampleSystemSecurityOperation.EmployeePersonalCellPhoneView)
                                                                       .SetEdit(SampleSystemSecurityOperation.EmployeePersonalCellPhoneEdit))

                   .AddDomainSecurity<TestRootSecurityObj>(b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                                                                 .SetPath(SecurityPath<TestRootSecurityObj>.Create(v => v.BusinessUnit).And(v => v.Location)))

                   .AddDomainSecurity<TestPerformanceObject>(b => b.SetView(SampleSystemSecurityRole.TestPerformance)
                                                                   .SetPath(
                                                                       SecurityPath<TestPerformanceObject>.Create(v => v.Location, true)
                                                                                                          .And(v => v.Employee, true)
                                                                                                          .And(v => v.BusinessUnit, true)
                                                                                                          .And(v => v.ManagementUnit, true)))

                   .AddDomainSecurity<TestPlainAuthObject>(b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                                                                 .SetPath(
                                                                     SecurityPath<TestPlainAuthObject>.Create(v => v.Location)
                                                                                                      .And(
                                                                                                          SecurityPath<TestPlainAuthObject>.CreateNested(
                                                                                                              v => v.Items,
                                                                                                              SecurityPath<TestItemAuthObject>.Create(i => i.BusinessUnit)
                                                                                                                  .And(i => i.ManagementUnit)))))

                   .AddDomainSecurity<AuthPerformanceObject>(b => b.SetView(SampleSystemSecurityRole.TestPerformance)
                                                                   .SetPath(
                                                                       SecurityPath<AuthPerformanceObject>.Create(v => v.BusinessUnit)
                                                                                                          .And(v => v.ManagementUnit)
                                                                                                          .And(v => v.Location)
                                                                                                          .And(v => v.Employee)))

                   .AddDomainSecurity<EmployeePhoto>(b => b.SetView(SampleSystemSecurityOperation.EmployeeView)
                                                           .SetPath(SecurityPath<EmployeePhoto>.Create(employeePhoto => employeePhoto.Employee.CoreBusinessUnit)))

                   .AddDomainSecurity<ManagementUnitFluentMapping>(b => b.SetView(SampleSystemSecurityOperation.ManagementUnitView)
                                                                         .SetEdit(SampleSystemSecurityOperation.ManagementUnitEdit))

                   .AddDomainSecurity<Example1>(b => b.SetView(SampleSystemSecurityOperation.LocationView)
                                                      .SetEdit(SampleSystemSecurityOperation.LocationEdit))

                   .AddDomainSecurity<TestRestrictionObject>(new[] { SampleSystemSecurityRole.RestrictionRole }, SecurityPath<TestRestrictionObject>.Create(v => v.BusinessUnit))

                   .AddDomainSecurity<TestCustomContextSecurityObj>(b => b.SetCustomService<SampleSystemTestCustomContextSecurityObjSecurityService>())

                   .AddDomainSecurity<TestSecurityObjItem>(b => b.SetDependency(v => v.FirstMaster))
                   .AddDomainSecurity<TestSecuritySubObjItem>(b => b.SetDependency(v => v.InnerMaster))
                   .AddDomainSecurity<TestSecuritySubObjItem2>(b => b.SetDependency(v => v.RootSecurityObj))
                   .AddDomainSecurity<TestSecuritySubObjItem3>(b => b.SetDependency(v => v.InnerMaster.FirstMaster));
        }

        private ISecuritySystemSettings RegisterDisabledDomainSecurityServices()
        {
            return settings
                   .AddDomainSecurity<EmployeeInformation>(SecurityRule.Disabled)
                   .AddDomainSecurity<EmployeeRegistrationType>(SecurityRule.Disabled)
                   .AddDomainSecurity<IMRequest>(SecurityRule.Disabled)
                   .AddDomainSecurity<Information>(SecurityRule.Disabled)
                   .AddDomainSecurity<Location1676>(SecurityRule.Disabled)
                   .AddDomainSecurity<WorkingCalendar1676>(SecurityRule.Disabled)
                   .AddDomainSecurity<Principal>(SecurityRule.Disabled)
                   .AddDomainSecurity<SqlParserTestObj>(SecurityRule.Disabled)
                   .AddDomainSecurity<SqlParserTestObjContainer>(SecurityRule.Disabled)

                   .AddDomainSecurity<TestImmutableObj>(SecurityRule.Disabled, SecurityRule.Disabled);
        }

        private ISecuritySystemSettings RegisterLegacyProjectionDomainSecurityServices()
        {
            return settings.AddDomainSecurity<SecurityBusinessUnit>(b => b.SetUntypedDependency<BusinessUnit>())

                           .AddDomainSecurity<SecurityEmployee>(b => b.SetUntypedDependency<Employee>())

                           .AddDomainSecurity<SecurityHRDepartment>(b => b.SetUntypedDependency<HRDepartment>())

                           .AddDomainSecurity<SecurityLocation>(b => b.SetUntypedDependency<Location>())

                           .AddDomainSecurity<TestLegacyEmployee>(b => b.SetUntypedDependency<Employee>());
        }
    }
}
