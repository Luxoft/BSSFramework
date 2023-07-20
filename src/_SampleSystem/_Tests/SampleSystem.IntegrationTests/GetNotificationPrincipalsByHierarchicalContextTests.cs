using System.Reflection;

using Automation.ServiceEnvironment;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;

using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class GetNotificationPrincipalsByHierarchicalContextTests : TestBase
{
    [TestMethod]
    public void GetPrincipalsWithPermissionToTwoRootUnits_By_Roles()
    {
        // Arrange
        var employeeLogin = "employee";

        var parentLocation = this.DataHelper.SaveLocation();
        var childLocation = this.DataHelper.SaveLocation(parent: parentLocation);

        var parentBusinessUnit = this.DataHelper.SaveBusinessUnit();
        var childBusinessUnit = this.DataHelper.SaveBusinessUnit(parent: parentBusinessUnit);

        var permissionToRootUnits = new SampleSystemPermission(TestBusinessRole.SystemIntegration, parentBusinessUnit, null, parentLocation);
        this.AuthHelper.SetUserRole(employeeLogin, permissionToRootUnits);

        var fbuChildFilter = new NotificationFilterGroup(typeof(BusinessUnit), new[] { childBusinessUnit.Id }, NotificationExpandType.DirectOrFirstParent);
        var locChildFilter = new NotificationFilterGroup(typeof(Location), new[] { childLocation.Id }, NotificationExpandType.DirectOrFirstParent);
        var securityFilters = new[] { fbuChildFilter, locChildFilter };

        var roleId = this.EvaluateRead(
            c => c.Authorization.Logics.BusinessRole.GetByName(permissionToRootUnits.GetRoleName()).Id);

        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            context => context.Authorization
                              .NotificationPrincipalExtractor
                              .GetNotificationPrincipalsByRoles(new[] { roleId }, securityFilters)
                              .ToArray(p => p.Name));

        this.Evaluate(
           DBSessionMode.Read,
           ctx =>
           {
               var permissionQ = ctx.Authorization
                                    .Logics
                                    .Permission
                                    .GetUnsecureQueryable()
                                    .Where(
                                        ctx.Authorization.NotificationPrincipalExtractor.GetRoleBaseNotificationFilter(new[] { roleId }));


               var startPermissionQ = permissionQ.Select(p => new PermissionLevelInfo { Permission = p, LevelInfo = "" });

               var res = securityFilters.Aggregate(
                                            startPermissionQ,
                                            (query, secGroup) => this.ApplyNotificationFilter(
                                                ctx.ServiceProvider,
                                                ctx.HierarchicalObjectExpanderFactory,
                                                query,
                                                secGroup))
                                        .ToList();

               return;
           });

        // Assert
        result.Length.Should().Be(1);
        result.Single().Should().Be(employeeLogin);
    }

    private IQueryable<PermissionLevelInfo> ApplyNotificationFilter(
        IServiceProvider serviceProvider,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IQueryable<PermissionLevelInfo> source,
        NotificationFilterGroup notificationFilterGroup)
    {
        var genericMethod = typeof(GetNotificationPrincipalsByHierarchicalContextTests).GetMethod(
            nameof(this.TypedApplyNotificationFilter),
            BindingFlags.Instance | BindingFlags.NonPublic);

        var method = genericMethod.MakeGenericMethod(notificationFilterGroup.EntityType);

        return method.Invoke<IQueryable<PermissionLevelInfo>>(this, serviceProvider, hierarchicalObjectExpanderFactory, source, notificationFilterGroup);
    }

    private IQueryable<PermissionLevelInfo> TypedApplyNotificationFilter<TSecurityContext>(
        IServiceProvider serviceProvider,
        IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
        IQueryable<PermissionLevelInfo> source,
        NotificationFilterGroup notificationFilterGroup)
        where TSecurityContext : PersistentDomainObjectBase, ISecurityContext, IHierarchicalLevelObject
    {

        var expandedSecIdents = notificationFilterGroup.ExpandType.IsHierarchical()
                                    ? hierarchicalObjectExpanderFactory.Create(notificationFilterGroup.EntityType).Expand(notificationFilterGroup.Idents, HierarchicalExpandType.Parents)
                                    : notificationFilterGroup.Idents;

        var grandAccess = notificationFilterGroup.ExpandType.AllowEmpty();

        var securityContextQ = serviceProvider.GetRequiredService<IRepositoryFactory<TSecurityContext>>().Create().GetQueryable();

        return from permissionInfo in source

               let permission = permissionInfo.Permission

               let permissionSecurityContextItems = securityContextQ.Where(
                   securityContext => permission.FilterItems
                                                 .Any(fi => fi.EntityType.Name == typeof(TSecurityContext).Name && fi.ContextEntityId == securityContext.Id))


               let directLevel = permissionSecurityContextItems.Where(securityContext => expandedSecIdents.Contains(securityContext.Id))
                                                                 .Select(secItem => (int?)secItem.DeepLevel).Max()
                                   ?? -2

               let grandLevel = grandAccess && permission.FilterItems.All(fi => fi.EntityType.Name != typeof(TSecurityContext).Name)
                                      ? -1
                                      : -2

               let level = Math.Max(directLevel, grandLevel)

               where level != -2

               select new PermissionLevelInfo
                      {
                          Permission = permission,
                          LevelInfo = permissionInfo.LevelInfo + $"|{typeof(TSecurityContext).Name}:{level}"
                      };
    }
}

public class PermissionLevelInfo
{
    public Permission Permission { get; set; }

    public string LevelInfo { get; set; }
}
