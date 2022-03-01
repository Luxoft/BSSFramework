using System;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.OData;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Inline;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.WebApiCore.Controllers.Main;

using BusinessRole = WorkflowSampleSystem.IntegrationTests.__Support.Utils.BusinessRole;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class DependencySecurityTests : TestBase
    {
        private const string TestEmployeeLogin = "DS SecurityTester";

        private const string TestObj1 = nameof(TestObj1);

        private const string TestObj2 = nameof(TestObj2);

        private const string TestObjItem1 = nameof(TestObjItem1);

        private const string TestObjItem2 = nameof(TestObjItem2);

        private const string TestObjSubItem1 = nameof(TestObjSubItem1);

        private const string TestObjSubItem2 = nameof(TestObjSubItem2);

        private const string TestObjSubItem3 = nameof(TestObjSubItem3);

        private BusinessUnitIdentityDTO bu1Ident;

        private BusinessUnitIdentityDTO bu2Ident;

        [TestInitialize]
        public void SetUp()
        {
            this.bu1Ident = this.DataHelper.SaveBusinessUnit();

            this.bu2Ident = this.DataHelper.SaveBusinessUnit();

            this.DataHelper.SaveEmployee(login: TestEmployeeLogin);

            this.AuthHelper.SetUserRole(TestEmployeeLogin, new WorkflowSampleSystemPermission(BusinessRole.Administrator, this.bu2Ident, null, null));

            this.EvaluateWrite(
                context =>
                {
                    var firstRootObj = new TestRootSecurityObj { Name = TestObj1, BusinessUnit = context.Logics.BusinessUnit.GetById(this.bu1Ident.Id, true) };
                    new TestSecurityObjItem(firstRootObj) { Name = TestObjItem1 };

                    var secondRootObj = new TestRootSecurityObj { Name = TestObj2, BusinessUnit = context.Logics.BusinessUnit.GetById(this.bu2Ident.Id, true) };
                    var secondItemObj = new TestSecurityObjItem(secondRootObj) { Name = TestObjItem2 };
                    new TestSecuritySubObjItem(secondItemObj) { Name = TestObjSubItem1 };
                    new TestSecuritySubObjItem2(secondItemObj) { Name = TestObjSubItem2 };
                    new TestSecuritySubObjItem3(secondItemObj) { Name = TestObjSubItem3 };

                    context.Logics.TestRootSecurityObj.Save(firstRootObj);
                    context.Logics.TestRootSecurityObj.Save(secondRootObj);
                });
        }

        [TestMethod]
        public void TestSecurityObjItem_LoadedByDependencySecurity()
        {
            // Arrange
            var testSecurityObjItemController = this.GetController<TestSecurityObjItemController>(TestEmployeeLogin);

            // Act
            var items = testSecurityObjItemController.GetVisualTestSecurityObjItems().ToList();

            // Assert
            items.Count().Should().Be(1);
            items[0].Name.Should().Be(TestObjItem2);
        }

        [TestMethod]
        public void TestSecurityObjItemProjection_LoadedByDependencySecurity()
        {
            // Arrange
            var testSecurityObjItemController = this.GetController<TestSecurityObjItemController>(TestEmployeeLogin);

            // Act
            var items = testSecurityObjItemController.GetTestSecurityObjItemProjections().ToList();

            // Assert
            items.Count().Should().Be(1);
            items[0].Name.Should().Be(TestObjItem2);
        }

        [TestMethod]
        public void TestSecurityObjSubItem1_LoadedByDependencySecurity()
        {
            // Arrange
            var testSecuritySubObjItemController = this.GetController<TestSecuritySubObjItemController>(TestEmployeeLogin);

            // Act
            var items = testSecuritySubObjItemController.GetVisualTestSecuritySubObjItems().ToList();

            // Assert
            items.Count().Should().Be(1);
            items[0].Name.Should().Be(TestObjSubItem1);
        }

        [TestMethod]
        public void TestSecurityObjSubItem2_LoadedByDependencySecurity()
        {
            // Arrange
            var testSecuritySubObjItem2Controller = this.GetController<TestSecuritySubObjItem2Controller>(TestEmployeeLogin);

            // Act
            var items = testSecuritySubObjItem2Controller.GetVisualTestSecuritySubObjItem2s().ToList();

            // Assert
            items.Count().Should().Be(1);
            items[0].Name.Should().Be(TestObjSubItem2);
        }

        [TestMethod]
        public void TestSecurityObjSubItem3_LoadedByDependencySecurity()
        {
            // Arrange
            var testSecuritySubObjItem3Controller = this.GetController<TestSecuritySubObjItem3Controller>(TestEmployeeLogin);

            // Act
            var items = testSecuritySubObjItem3Controller.GetVisualTestSecuritySubObjItem3s().ToList();

            // Assert
            items.Count().Should().Be(1);
            items[0].Name.Should().Be(TestObjSubItem3);
        }
    }
}
