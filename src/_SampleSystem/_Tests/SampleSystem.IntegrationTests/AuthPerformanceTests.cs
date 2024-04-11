using Automation.ServiceEnvironment;
using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;

using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;

namespace SampleSystem.IntegrationTests.Workflow;

[TestClass]
public class AuthPerformanceTests : TestBase
{
    private const string TestUser = "TestUser";

    private const int Limit = 3;

    private const int SplitBy = 25;

    [TestInitialize]
    public void SetUp()
    {
        var genLoc = Enumerable.Range(0, Limit).ToList(i => this.DataHelper.SaveLocation());

        var genEmployee = Enumerable.Range(0, Limit).ToList(i => this.DataHelper.SaveEmployee());

        var genBu = Enumerable.Range(0, Limit).ToList(i => this.DataHelper.SaveBusinessUnit());

        var genMbu = Enumerable.Range(0, Limit).ToList(i => this.DataHelper.SaveManagementUnit());

        var genObjects = this.EvaluateWrite(ctx =>
                                            {
                                                var gebObjectsRequest =
                                                        from emplIdent in genEmployee
                                                        from locIdent in genLoc
                                                        from buIdent in genBu
                                                        from mbuIdent in genMbu
                                                        select new TestPerformanceObject
                                                               {
                                                                       Employee = ctx.Logics.Employee.GetById(emplIdent.Id),
                                                                       Location = ctx.Logics.Location.GetById(locIdent.Id),
                                                                       BusinessUnit = ctx.Logics.BusinessUnit.GetById(buIdent.Id),
                                                                       ManagementUnit = ctx.Logics.ManagementUnit.GetById(mbuIdent.Id),
                                                                       Name = Guid.NewGuid().ToString()
                                                               };

                                                var genObjects = gebObjectsRequest.ToList();

                                                ctx.Logics.TestPerformanceObject.Save(genObjects);

                                                var testPrincipal = ctx.Authorization.Logics.Principal.GetByNameOrCreate(TestUser);

                                                var pfeCache = new DictionaryCache<PersistentDomainObjectBase, PermissionFilterEntity>(domainObj =>
                                                {
                                                    var entityType = ctx.Authorization.GetEntityType(domainObj.GetType());

                                                    return ctx.Authorization.Logics.PermissionFilterEntity.GetOrCreate(entityType, new SecurityEntity { Id = domainObj.Id });
                                                });

                                                var adminRole = ctx.Authorization.Logics.BusinessRole.GetAdminRole();

                                                foreach (var genObjectSubEnumerable in genObjects.Split(SplitBy))
                                                {
                                                    var genPermission = new Permission(testPrincipal) { Role = adminRole };

                                                    foreach (var genObject in genObjectSubEnumerable)
                                                    {
                                                        if (!genPermission.Restrictions.Select(fi => fi.Entity.EntityId).Contains(genObject.Employee.Id))
                                                        {
                                                            new PermissionRestriction(genPermission, pfeCache[genObject.Employee]);
                                                        }

                                                        if (!genPermission.Restrictions.Select(fi => fi.Entity.EntityId).Contains(genObject.Location.Id))
                                                        {
                                                            new PermissionRestriction(genPermission, pfeCache[genObject.Location]);
                                                        }

                                                        if (!genPermission.Restrictions.Select(fi => fi.Entity.EntityId).Contains(genObject.BusinessUnit.Id))
                                                        {
                                                            new PermissionRestriction(genPermission, pfeCache[genObject.BusinessUnit]);
                                                        }

                                                        if (!genPermission.Restrictions.Select(fi => fi.Entity.EntityId).Contains(genObject.ManagementUnit.Id))
                                                        {
                                                            new PermissionRestriction(genPermission, pfeCache[genObject.ManagementUnit]);
                                                        }
                                                    }
                                                }

                                                ctx.Authorization.Logics.Principal.Save(testPrincipal);

                                                return genObjects.Select(obj => obj.Id);
                                            });
    }

    [TestMethod]
    public void CreateObjectsWithPermissions_HasAccessToAllObjects()
    {
        // Arrange
        var testController = this.GetControllerEvaluator<TestPerformanceObjectController>(TestUser);

        // Act
        testController.Evaluate(c => c.GetSimpleTestPerformanceObjects());
        testController.Evaluate(c => c.GetSimpleTestPerformanceObjects());

        var start = DateTime.Now;

        var testPerformanceObjects = testController.Evaluate(c => c.GetSimpleTestPerformanceObjects());

        var duration = DateTime.Now - start;

        Console.WriteLine("WorkTime: " + duration);

        // Assert
        testPerformanceObjects.Count().Should().Be(Limit * Limit * Limit * Limit);
    }
}
