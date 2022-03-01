using System;
using System.Linq;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Security;
using Framework.SecuritySystem;
using Framework.Transfering;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers.Main;

namespace SampleSystem.BLL.Test
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public void TestGetProjectionViewOperationByMode()
        {
            var x = SampleSystemSecurityOperation.GetByMode<CustomCompanyLegalEntity>(BLLSecurityMode.View);

            return;
        }

        [TestMethod]
        public void TestExtractQueryableFromSecPath()
        {
            TestServiceEnvironment.IntegrationEnvironment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var bll = context.Logics.BusinessUnitHrDepartmentFactory.Create(BLLSecurityMode.View);

                var items = bll.GetFullList();

                return;
            });
        }

        [TestMethod]
        public void TestPermissionDuplicates()
        {
            IAuthorizationServiceEnvironment environment = TestServiceEnvironment.IntegrationEnvironment;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var entity = context.Logics.PermissionFilterEntity.GetObjectBy(fe => fe.EntityId == new Guid("38BFD64B-9268-464E-A8AB-0276BBEB1631"));

                var role = context.Logics.BusinessRole.GetAdminRole();

                var period = context.DateTimeService.CurrentMonth;

                Enumerable.Range(0, 2).Foreach(_ =>
                {
                    var newPermission = new Permission(context.CurrentPrincipal) { Role = role, Period = period };

                    new PermissionFilterItem(newPermission) { Entity = entity };
                });

                context.Logics.Principal.Save(context.CurrentPrincipal);

                return;
            });
        }

        [TestMethod]
        public void TestLoadBY()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, "Tester01", context =>
            {
                var tree = context.Logics.TestBusinessUnitFactory.Create(BLLSecurityMode.View).GetTree();

                return;
            });
        }
        
        [TestMethod]
        public void TestLoadProjections()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            var facade = environment.GetController<HRDepartmentController>("Tester01");

            var res1 = facade.GetTestDepartmentsByOperation(SampleSystemHRDepartmentSecurityOperationCode.EmployeeEdit);

            //var res2 = queryFacade.GetProjectionSimpleBusinessUnitsByODataQueryString("$top=10");

            return;
        }

        [TestMethod]
        public void TestConvertSecurityPath()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, "Tester01", (ISampleSystemBLLContext context) =>
            {
                var employees = context.Logics.TestEmployee.GetFullList();

                var provider = context.SecurityService.GetSecurityProvider<TestEmployee>(SampleSystemSecurityOperationCode.EmployeeView);

                foreach (var employee in employees)
                {
                    var hasAccess = provider.HasAccess(employee);

                    continue;
                }

                ////var baseProvider = context.SecurityService.GetSecurityProvider<Employee>(BLLSecurityMode.View);

                ////var projectionProvider = context.SecurityService.GetSecurityProvider<SimpleEmployee>(BLLSecurityMode.View);

                ////var zz1 = context.Logics.EmployeeFactory.Create(BLLSecurityMode.View).GetFullList();

                ////var zz2 = context.Logics.SimpleEmployeeFactory.Create(BLLSecurityMode.View).GetFullList(MainDTOType.RichDTO);

                return;
            });
        }

        [TestMethod]
        public void ClearAuthDB()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                context.Authorization.Logics.Default.Create<Framework.Authorization.Domain.Principal>().Remove(context.Authorization.Logics.Principal.GetFullList());

                context.Authorization.Logics.PermissionFilterEntity.Remove(context.Authorization.Logics.PermissionFilterEntity.GetFullList());

                context.Authorization.Logics.EntityType.Remove(context.Authorization.Logics.EntityType.GetFullList());

                return;
            });
        }

        [TestMethod]
        public void AddEntityTypes()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                context.Authorization.Logics.EntityType.Register(new[] { typeof(BusinessUnit).Assembly });

                return;
            });
        }

        [TestMethod]
        public void AddSelfAdminPermissions()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var adminRole = context.Authorization.Logics.BusinessRole.GetOrCreateAdminRole();

                var principal = context.Authorization.Logics.Principal.GetCurrent(true);

                if (principal.Permissions.All(p => p.Role != adminRole))
                {
                    context.Authorization.Logics.Permission.Save(new Permission(principal) { Role = adminRole }, false);
                }

                return;
            });
        }

        [TestMethod]
        public void TestRemoveWithDelegate()
        {
            var environment = TestServiceEnvironment.IntegrationEnvironment;

            var principalName = "TestDelegateUser";

            var roleName = "TestDelegateRole";

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, (context, session) =>
            {
                var testRole = context.Authorization.Logics.BusinessRole.GetByNameOrCreate(roleName, true);

                var testPrincipal = context.Authorization.Logics.Principal.GetByNameOrCreate(principalName, true);

                var permissionForDelegate = testPrincipal.Permissions.SingleOrDefault(p => p.Role == testRole) ??
                                            new Permission(testPrincipal) { Role = testRole }.Self(context.Authorization.Logics.Permission.Save);

                session.Flush();

                var currentPrincipal = context.Authorization.Logics.Principal.GetCurrent(true);

                if (permissionForDelegate.DelegatedTo.All(delPerm => delPerm.Principal != currentPrincipal))
                {
                    new Permission(currentPrincipal, permissionForDelegate) { Role = testRole }.Self(context.Authorization.Logics.Permission.Save);
                }
            });

            environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var testPrincipal = context.Authorization.Logics.Principal.GetByName(principalName, true);

                context.Authorization.Logics.Principal.Remove(testPrincipal, true);

                var currentPrincipal = context.Authorization.Logics.Principal.GetCurrent(true);

                var zzz = currentPrincipal.Permissions.ToList();
            });
        }
    }
}
