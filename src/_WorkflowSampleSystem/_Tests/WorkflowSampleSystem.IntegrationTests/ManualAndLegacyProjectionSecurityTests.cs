using System;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.OData;
using Framework.SecuritySystem;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Inline;
using WorkflowSampleSystem.Domain.Projections;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.Domain.ManualProjections;
using WorkflowSampleSystem.WebApiCore.Controllers.MainQuery;

using BusinessRole = WorkflowSampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class ManualAndLegacyProjectionSecurityTests : TestBase
    {
        private const string TestEmployeeLogin = "MALProjection SecurityTester";

        private EmployeeIdentityDTO TestEmp1;

        private EmployeeIdentityDTO TestEmp2;


        private BusinessUnitIdentityDTO bu1Ident;

        private BusinessUnitIdentityDTO bu2Ident;

        [TestInitialize]
        public void SetUp()
        {
            this.bu1Ident = this.DataHelper.SaveBusinessUnit();

            this.bu2Ident = this.DataHelper.SaveBusinessUnit();

            this.DataHelper.SaveEmployee(login: TestEmployeeLogin);

            this.AuthHelper.SetUserRole(TestEmployeeLogin, new WorkflowSampleSystemPermission(BusinessRole.Administrator, this.bu2Ident, null, null));

            this.TestEmp1 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu1Ident);

            this.TestEmp2 = this.DataHelper.SaveEmployee(coreBusinessUnit: this.bu2Ident);
        }

        [TestMethod]
        public void TestManualEmployeeProjection_LoadedByManualDependencySecurity()
        {
            // Arrange
            var employeeQueryController = this.GetController<EmployeeQueryController>(TestEmployeeLogin);

            // Act
            var items = employeeQueryController.GetTestManualEmployeeProjectionsByODataQueryString($"$filter={nameof(TestManualEmployeeProjection.CoreBusinessUnitId)} ne null").Items;

            // Assert
            items.Count().Should().Be(1);
            items[0].Identity.Should().Be(this.TestEmp2);
        }

        [TestMethod]
        public void TestLegacyEmployeeProjection_LoadedByLegacyGenericSecurity()
        {
            // Arrange
            var employeeQueryController = this.GetController<EmployeeQueryController>(TestEmployeeLogin);

            // Act
            var items = employeeQueryController.GetTestLegacyEmployeesByODataQueryString($"$filter={nameof(TestLegacyEmployee.BusinessUnit_Security)} ne null").Items;

            // Assert
            items.Count().Should().Be(1);
            items[0].Identity.Should().Be(this.TestEmp2);
        }


        [TestMethod]
        public void TestLegacyEmployeeProjection_AccessorsResolved()
        {
            // Arrange

            // Act
            var items = this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Read, TestEmployeeLogin, ctx =>
            {
                var bll = ctx.Logics.TestLegacyEmployeeFactory.Create(BLLSecurityMode.View);

                return bll.GetListBy(v => v.BusinessUnit_Security != null).ToDictionary(v => v.Id, bll.SecurityProvider.GetAccessors);
            });

            // Assert
            items.Count().Should().Be(1);
            var item = items.Single();
            item.Key.Should().Be(this.TestEmp2.Id);

            item.Value.Should().Contain(TestEmployeeLogin);
        }
    }
}
