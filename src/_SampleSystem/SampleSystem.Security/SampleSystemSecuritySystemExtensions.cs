using Framework.Core;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;
using SampleSystem.Domain.EnversBug1676;
using SampleSystem.Domain.ManualProjections;
using SampleSystem.Security.Metadata;
using SampleSystem.Security.Services;

namespace SampleSystem.Security;

public static class SampleSystemSecuritySystemExtensions
{
    public static ISecuritySystemSettings AddSecurityContexts(this ISecuritySystemSettings settings)
    {
        return settings.AddSecurityContext<BusinessUnit>(new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"))
                       .AddSecurityContext<Location>(new Guid("4641395B-9079-448E-9CB8-A083015235A3"))
                       .AddSecurityContext<ManagementUnit>(new Guid("77E78AEF-9512-46E0-A33D-AAE58DC7E18C"))
                       .AddSecurityContext<Employee>(new Guid("B3F2536E-27C4-4B91-AE0B-0EE2FFD4465F"), displayFunc: employee => employee.Login);
    }

    public static ISecuritySystemSettings AddSecurityRoles(this ISecuritySystemSettings settings)
    {
        return settings
               .AddSecurityRole(
                   SampleSystemSecurityRole.SecretariatNotification,
                   new SecurityRoleInfo(new Guid("8fd79f66-218a-47bc-9649-a07500fa6d11")))

               .AddSecurityRole(
                   SampleSystemSecurityRole.SeManager,
                   new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeeView]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.TestRole1,
                   new SecurityRoleInfo(new Guid("{597AAB2A-76F7-42CF-B606-3D4550062596}"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeeView]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.TestRole2,
                   new SecurityRoleInfo(new Guid("{AD5EC94F-CC3D-451B-9051-B83059707E11}"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeePositionView]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.TestRole3,
                   new SecurityRoleInfo(new Guid("{B1B30E65-36BF-4ED1-9BD1-E614BA349507}"))
                   {
                       Operations = [SampleSystemSecurityOperation.EmployeeEdit]
                   })

               .AddSecurityRole(
                   SampleSystemSecurityRole.SearchTestBusinessRole,
                   new SecurityRoleInfo(new Guid("{05271C71-7E6B-430A-9EC7-F838845D0F33}")))

               .AddSecurityRole(
                   SampleSystemSecurityRole.RestrictionRole,
                   new SecurityRoleInfo(new Guid("{ACAA7B42-09AA-438A-B6EC-058506E0C103}"))
                   {
                       Restriction = SecurityPathRestriction.Create<BusinessUnit>()
                                                            .Add((TestRestrictionObject v) => v.RestrictionHandler)
                   })

               .AddSecurityRole(
                   SecurityRole.SystemIntegration,
                   new SecurityRoleInfo(new Guid("df74d544-5945-4380-944e-a3a9001252be")))

               .AddSecurityRole(
                   SecurityRole.Administrator,
                   new SecurityRoleInfo(new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e"))
                   {
                       Operations = typeof(SampleSystemSecurityOperation).GetStaticPropertyValueList<SecurityOperation>().ToList()
                   })

               .AddSecurityOperation(SampleSystemSecurityOperation.BusinessUnitView, new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All })

               .AddSecurityOperation(SampleSystemSecurityOperation.ManagementUnitView, new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All });
    }


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
                                                                 .And(SecurityPath<TestPlainAuthObject>.CreateNested(v => v.Items,
                                                                      SecurityPath<TestItemAuthObject>.Create(i => i.BusinessUnit).And(i => i.ManagementUnit),
                                                                      ManySecurityPathMode.Any))))

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
                      b => b.SetUntypedDependency<ManagementUnit>())

                  .Add<Example1>(
                      b => b.SetView(SampleSystemSecurityOperation.LocationView)
                            .SetEdit(SampleSystemSecurityOperation.LocationEdit))

                  .Add<TestRestrictionObject>(SampleSystemSecurityRole.RestrictionRole, SecurityPath<TestRestrictionObject>.Create(v => v.BusinessUnit))

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
