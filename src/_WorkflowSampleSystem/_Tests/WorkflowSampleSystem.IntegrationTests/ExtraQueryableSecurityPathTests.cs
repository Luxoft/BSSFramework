using System;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.OData;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Inline;
using WorkflowSampleSystem.Domain.Projections;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.Domain.ManualProjections;



using BusinessRole = WorkflowSampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class ExtraQueryableSecurityPathTests : TestBase
    {
        private const string TestEmployeeLogin = "EQSP SecurityTester";

        private EmployeeIdentityDTO TestEmp1;

        private EmployeeIdentityDTO TestEmp2;

        private EmployeeIdentityDTO TestEmp3;


        private BusinessUnitIdentityDTO bu1Ident;

        private BusinessUnitIdentityDTO bu2Ident;

        private LocationIdentityDTO loc1Ident;

        private LocationIdentityDTO loc2Ident;

        [TestInitialize]
        public void SetUp()
        {
            this.bu1Ident = this.DataHelper.SaveBusinessUnit();

            this.bu2Ident = this.DataHelper.SaveBusinessUnit();

            this.loc1Ident = this.DataHelper.SaveLocation(name: "Loc 1 (ExtraQueryableSecurityPathTests)");

            this.loc2Ident = this.DataHelper.SaveLocation(name: "Loc 2 (ExtraQueryableSecurityPathTests)");

            this.DataHelper.SaveEmployee(login: TestEmployeeLogin);

            this.AuthHelper.SetUserRole(TestEmployeeLogin, new WorkflowSampleSystemPermission(BusinessRole.Administrator, this.bu2Ident, null, this.loc1Ident));
            this.AuthHelper.AddUserRole(TestEmployeeLogin, new WorkflowSampleSystemPermission(BusinessRole.Administrator, this.bu2Ident, null, this.loc2Ident));

            this.TestEmp1 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu1Ident, location: this.loc1Ident);

            this.TestEmp2 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu2Ident, location: this.loc1Ident);

            this.TestEmp3 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu2Ident, location: this.loc2Ident);
        }

        [TestMethod]
        public void TestExtraQueryableSecurityPath_LoadedWithExtraQueryableFilter()
        {
            // Arrange
            var createProviderFunc = FuncHelper.Create((IWorkflowSampleSystemBLLContext context) =>
            {
                var extraQueryableSecurity = context.Logics.Location.GetUnsecureQueryable().Where(l => l.Id == this.loc1Ident.Id);

                var extraSecurityPath = WorkflowSampleSystemSecurityPath<Employee>.Create(e => e.CoreBusinessUnit, SingleSecurityMode.Strictly)
                                                                          .And(e => e.Location, SingleSecurityMode.Strictly)
                                                                          .And(_ => extraQueryableSecurity, ManySecurityPathMode.Any);

                return extraSecurityPath.ToProvider(WorkflowSampleSystemSecurityOperation.EmployeeView, context.SecurityExpressionBuilderFactory, context.AccessDeniedExceptionService);
            });

            // Act
            var items = this.Environment.GetContextEvaluator().Evaluate (DBSessionMode.Read, TestEmployeeLogin, context =>
            {
                var employees = context.Logics.EmployeeFactory.Create(createProviderFunc(context)).GetSecureQueryable().ToList();

                return employees.ToArray(e => e.Id);
            });

            // Assert
            items.Count().Should().Be(1);
            items[0].Should().Be(this.TestEmp2.Id);
        }
    }
}
