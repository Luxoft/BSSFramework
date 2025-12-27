using Framework.Core;
using Framework.DomainDriven.Setup;

using SecuritySystem;

using SampleSystem.Domain;

using SecuritySystem.DependencyInjection;
using SecuritySystem.VirtualPermission.DependencyInjection;

using HierarchicalExpand;

namespace SampleSystem.Security;

public static class SampleSystemSecuritySystemExtensions
{
    extension(ISecuritySystemSettings settings)
    {
        public ISecuritySystemSettings AddSecurityContexts()
        {
            return settings.AddSecurityContext<BusinessUnit>(
                               new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"),
                               b => b.SetHierarchicalInfo(
                                         v => v.Parent,
                                         new AncestorLinkInfo<BusinessUnit, BusinessUnitAncestorLink>(link => link.Ancestor, link => link.Child),
                                         new AncestorLinkInfo<BusinessUnit, BusinessUnitToAncestorChildView>(link => link.Source, link => link.ChildOrAncestor))
                                     .SetDeepLevel(v => v.DeepLevel))
                           .AddSecurityContext<Location>(
                               new Guid("4641395B-9079-448E-9CB8-A083015235A3"),
                               b => b.SetHierarchicalInfo(
                                         v => v.Parent,
                                         new AncestorLinkInfo<Location, LocationAncestorLink>(link => link.Ancestor, link => link.Child),
                                         new AncestorLinkInfo<Location, LocationToAncestorChildView>(link => link.Source, link => link.ChildOrAncestor))
                                     .SetDeepLevel(v => v.DeepLevel))
                           .AddSecurityContext<ManagementUnit>(
                               new Guid("77E78AEF-9512-46E0-A33D-AAE58DC7E18C"),
                               b => b.SetHierarchicalInfo(
                                         v => v.Parent,
                                         new AncestorLinkInfo<ManagementUnit, ManagementUnitAncestorLink>(link => link.Ancestor, link => link.Child),
                                         new AncestorLinkInfo<ManagementUnit, ManagementUnitToAncestorChildView>(link => link.Source, link => link.ChildOrAncestor))
                                     .SetDeepLevel(v => v.DeepLevel))
                           .AddSecurityContext<Employee>(
                               new Guid("B3F2536E-27C4-4B91-AE0B-0EE2FFD4465F"),
                               b => b.SetDisplayFunc(employee => employee.Login));
        }

        public ISecuritySystemSettings AddSecurityRoles()
        {
            return settings
                   .AddSecurityRole(
                       SampleSystemSecurityRole.SecretariatNotification,
                       new SecurityRoleInfo(new Guid("8fd79f66-218a-47bc-9649-a07500fa6d11")))

                   .AddSecurityRole(
                       SampleSystemSecurityRole.SeManager,
                       new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"))
                       {
                           Children = [SampleSystemSecurityRole.TestVirtualRole], Operations = [SampleSystemSecurityOperation.BusinessUnitEdit]
                       })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.TestRole1,
                       new SecurityRoleInfo(new Guid("{597AAB2A-76F7-42CF-B606-3D4550062596}")) { Children = [SampleSystemSecurityRole.TestVirtualRole] })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.TestRole2,
                       new SecurityRoleInfo(new Guid("{AD5EC94F-CC3D-451B-9051-B83059707E11}")) { Operations = [SampleSystemSecurityOperation.EmployeePositionView] })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.TestRole3,
                       new SecurityRoleInfo(new Guid("{B1B30E65-36BF-4ED1-9BD1-E614BA349507}")) { Operations = [SampleSystemSecurityOperation.EmployeeEdit] })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.DefaultRole,
                       new SecurityRoleInfo(new Guid("{5D93F3E7-6750-47D2-A791-D285305D5E94}")))

                   .AddSecurityRole(
                       SampleSystemSecurityRole.SearchTestBusinessRole,
                       new SecurityRoleInfo(new Guid("{05271C71-7E6B-430A-9EC7-F838845D0F33}")))

                   .AddSecurityRole(
                       SampleSystemSecurityRole.RestrictionRole,
                       new SecurityRoleInfo(new Guid("{ACAA7B42-09AA-438A-B6EC-058506E0C103}"))
                       {
                           Restriction = SecurityPathRestriction.Create<BusinessUnit>()
                                                                .AddRelativeCondition<TestRestrictionObject>(obj => obj.RestrictionHandler)
                       })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.WithRestrictionFilterRole,
                       new SecurityRoleInfo(new Guid("{00645BD7-2D47-40E4-B542-E9A33EC06CB4}"))
                       {
                           Restriction = SecurityPathRestriction.Create<BusinessUnit>(required: true, filter: bu => bu.AllowedForFilterRole)
                       })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.RequiredRestrictionRole,
                       new SecurityRoleInfo(new Guid("{D2E95D39-D6E7-41E1-81B0-2EB0E50110FD}"))
                       {
                           Restriction = SecurityPathRestriction.Create<BusinessUnit>(true)
                                                                .Add<Location>(true)
                                                                .Add<ManagementUnit>()
                       })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.TestVirtualRole,
                       new SecurityRoleInfo(new Guid("{D75CFAB6-A089-4CEE-924E-0F057C627320}")) { IsVirtual = true, Operations = [SampleSystemSecurityOperation.EmployeeView] })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.TestVirtualRole2,
                       new SecurityRoleInfo(new Guid("{649DE6F3-A943-46A3-9E81-AA056D24B52D}")) { IsVirtual = true, })

                   .AddSecurityRole(
                       SampleSystemSecurityRole.TestPerformance,
                       new SecurityRoleInfo(new Guid("{B1A5B1B6-F92D-4367-B7EC-200179E80308}")))

                   .AddSecurityRole(
                       SampleSystemSecurityRole.PermissionAdministrator,
                       new SecurityRoleInfo(new Guid("{1E101597-E722-4650-BED1-5A1025540897}")))

                   .AddSecurityRole(
                       SecurityRole.SystemIntegration,
                       new SecurityRoleInfo(new Guid("df74d544-5945-4380-944e-a3a9001252be")))

                   .AddSecurityRole(
                       SecurityRole.Administrator,
                       new SecurityRoleInfo(new Guid("d9c1d2f0-0c2f-49ab-bb0b-de13a456169e"))
                       {
                           Operations = typeof(SampleSystemSecurityOperation).GetStaticPropertyValueList<SecurityOperation>().ToList()
                       });
        }

        public ISecuritySystemSettings AddSecurityRules()
        {
            DomainSecurityRule.RoleBaseSecurityRule g = new[] { SecurityRole.Administrator, };

            return settings.AddSecurityRule(
                               SampleSystemSecurityRule.TestRestriction,
                               SecurityRule.Disabled.And((TestRestrictionObject v) => v.RestrictionHandler))
                           .AddSecurityRule(
                               SampleSystemSecurityGroup.TestGroup,
                               SecurityRule.View.ToDomain<Employee>())
                           .AddSecurityRule(
                               SampleSystemSecurityRule.TestRoleGroup,
                               new[] { g });
        }

        public ISecuritySystemSettings AddCustomSecurityOperations()
        {
            return settings.AddSecurityOperation(
                               SampleSystemSecurityOperation.BusinessUnitView,
                               new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All })

                           .AddSecurityOperation(
                               SampleSystemSecurityOperation.ManagementUnitView,
                               new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All });
        }

        public ISecuritySystemSettings AddVirtualPermissions()
        {
            return settings.AddVirtualPermission<Employee, BusinessUnitEmployeeRole>(
                link => link.Employee,
                vpb => vpb.ForRole(
                    SampleSystemSecurityRole.SeManager,
                    v => v.AddRestriction(link => link.BusinessUnit)
                          .AddFilter(link => link.Role == BusinessUnitEmployeeRoleType.Manager)));
        }
    }
}
