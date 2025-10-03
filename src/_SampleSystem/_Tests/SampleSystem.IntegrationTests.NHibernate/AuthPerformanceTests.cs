using Automation.ServiceEnvironment;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.Main;

using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;
using Principal = Framework.Authorization.Domain.Principal;

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

                                                var testPrincipal = new Principal { Name = TestUser };

                                                var adminRole = ctx.Authorization.Logics.BusinessRole.GetByName(SampleSystemSecurityRole.TestPerformance.Name);

                                                foreach (var genObjectSubEnumerable in genObjects.Split(SplitBy))
                                                {
                                                    var genPermission = new Permission(testPrincipal) { Role = adminRole };

                                                    foreach (var genObject in genObjectSubEnumerable)
                                                    {
                                                        void tryAddRestrictions<TSecurityContext>(Func<TestPerformanceObject, TSecurityContext> getSecurityContext)
                                                            where TSecurityContext : PersistentDomainObjectBase, ISecurityContext
                                                        {
                                                            var securityContext = getSecurityContext(genObject);

                                                            if (!genPermission.Restrictions.Select(fi => fi.SecurityContextId).Contains(securityContext.Id))
                                                            {
                                                                new PermissionRestriction(genPermission)
                                                                {
                                                                    SecurityContextId = securityContext.Id,
                                                                    SecurityContextType = ctx.Authorization.GetSecurityContextType(typeof(TSecurityContext))
                                                                };
                                                            }
                                                        }

                                                        tryAddRestrictions(v => v.Employee);
                                                        tryAddRestrictions(v => v.Location);
                                                        tryAddRestrictions(v => v.BusinessUnit);
                                                        tryAddRestrictions(v => v.ManagementUnit);
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
