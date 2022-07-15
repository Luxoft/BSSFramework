using System;
using System.Linq.Expressions;

using FluentAssertions;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.IntegrationTests.__Support.Utils.Framework;
using SampleSystem.WebApiCore.Controllers.Main;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class BusinessUnitTests : TestBase
    {
        private const string EmployeeName = "TestSecondaryAccessEmployee";

        private const string EditEmployeeRoleName = "EditEmployeeRole";

        private const int ParentsCount = 2200;

        private const int ExistedBusinessUnitsInDatabase = 3;

        [TestInitialize]
        public void SetUp()
        {
            var buTypeId = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID);

            var luxoftBuId = this.DataHelper.SaveBusinessUnit(
                                                              id: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_ID,
                                                              name: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME,
                                                              type: buTypeId);

            this.DataHelper.SaveBusinessUnit(
                                             id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID,
                                             name: DefaultConstants.BUSINESS_UNIT_PARENT_CC_NAME,
                                             type: buTypeId,
                                             parent: luxoftBuId);

            this.DataHelper.SaveBusinessUnit(
                                             id: DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID,
                                             name: DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME,
                                             type: buTypeId,
                                             parent: luxoftBuId);

            this.DataHelper.Environment.GetContextEvaluator().Evaluate(
                                                 DBSessionMode.Write,
                                                 context =>
                                                 {
                                                     var authContext = context.Authorization;

                                                     var principalBll = authContext.Logics.Principal;
                                                     var principal = principalBll.GetByNameOrCreate(EmployeeName, true);

                                                     var entityType = authContext.Logics.EntityType.GetByName(nameof(BusinessUnit));

                                                     Expression<Func<PermissionFilterEntity, bool>> entitySearchFilter =
                                                             entity =>
                                                                 entity.EntityType == entityType
                                                                 && entity.EntityId == DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID;

                                                     var filterEntity = authContext.Logics.PermissionFilterEntity.GetObjectBy(entitySearchFilter) ?? new PermissionFilterEntity
                                                     {
                                                         EntityType = entityType,
                                                         EntityId = DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID
                                                     }.Self(bu => authContext.Logics.PermissionFilterEntity.Save(bu));

                                                     var permission = new Permission(principal);

                                                     permission.Role = authContext.Logics.BusinessRole.GetByName(EditEmployeeRoleName) ?? this.CreateEditEmployeeRole(authContext);

                                                     new PermissionFilterItem(permission) { Entity = filterEntity };

                                                     principalBll.Save(principal);
                                                 });
        }

        private BusinessRole CreateEditEmployeeRole(IAuthorizationBLLContext authContext)
        {
            var role = new BusinessRole { Name = EditEmployeeRoleName };

            var operation = authContext.Logics.Operation.GetByName(SampleSystemBusinessUnitSecurityOperationCode.EmployeeEdit.ToString());

            var link = new BusinessRoleOperationLink(role) { Operation = operation };

            authContext.Logics.BusinessRole.Save(role);

            return role;
        }

        [TestMethod]
        public void CheckBusinessUnitSecondaryAccess_HasAccess()
        {
            // Arrange
            var businessUnitQueryController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.MainQuery.BusinessUnitQueryController>(EmployeeName);

            // Act
            var businessUnitTree = businessUnitQueryController.Evaluate(c => c.GetTestBusinessUnitTreeByOperation(new GetTestBusinessUnitTreeByOperationAutoRequest
                {
                    odataQueryString = string.Empty,
                    securityOperationCode = SampleSystemBusinessUnitSecurityOperationCode.EmployeeEdit
                }));

            // Assert
            businessUnitTree.TotalCount.Should().Be(2);
            businessUnitTree.Items.Count.Should().Be(2);

            businessUnitTree.Items.Should().Contain(node => node.Item.Name == DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME && !node.OnlyView);
            businessUnitTree.Items.Should().Contain(node => node.Item.Name == DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME && node.OnlyView);
        }

        [TestMethod]
        public void GetTreeWithFilter()
        {
            // Arrange
            var businessUnitQueryController = this.GetControllerEvaluator<BusinessUnitQueryController>(EmployeeName);

            // Act
            var businessUnitTree = businessUnitQueryController.Evaluate(c => c.GetTestBusinessUnitTreeByOperation(new GetTestBusinessUnitTreeByOperationAutoRequest
                {
                    odataQueryString = "$filter=Name eq 'test'",
                    securityOperationCode = SampleSystemBusinessUnitSecurityOperationCode.EmployeeEdit
                }));

            // Assert
            businessUnitTree.TotalCount.Should().Be(0);
            businessUnitTree.Items.Count.Should().Be(0);
        }

        // Two test in one for performance reasons
        [TestMethod]
        public void GetFullBusinessUnitsTreeTwoTestInOne()
        {
            // Arrange
            this.CreateBigBuTree();

            this.TestGetFullBusinessUnitsTreeByOData();

            this.TestGetFullBusinessUnitsTree();
        }

        private void TestGetFullBusinessUnitsTreeByOData()
        {
            // Act
            var businessUnitQueryController = this.GetControllerEvaluator<BusinessUnitController>();
            var businessUnitTree = businessUnitQueryController.Evaluate(c => c.GetFullBusinessUnitsTreeByOData(string.Empty));

            // Assert
            businessUnitTree.TotalCount.Should().Be(ParentsCount + ExistedBusinessUnitsInDatabase);
            businessUnitTree.Items.Count.Should().Be(ParentsCount + ExistedBusinessUnitsInDatabase);
        }

        private void TestGetFullBusinessUnitsTree()
        {
            // Act
            var businessUnitController = this.GetControllerEvaluator<BusinessUnitController>();
            var tree = businessUnitController.Evaluate(c => c.GetFullBusinessUnitsTree());

            // Assert
            tree.Should().HaveCount(ParentsCount + ExistedBusinessUnitsInDatabase);
        }

        private void CreateBigBuTree()
        {
            var buAccountId = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_ACCOUNT_ID);

            this.DataHelper
                .EvaluateWrite(
                               context =>
                               {
                                   var period = new Period(this.GetDateTimeService().CurrentFinancialYear.StartDate
                                                                  .AddYears(-1));
                                   var accountType = context.Logics.BusinessUnitType.GetById(buAccountId.Id);

                                   for (var i = 0; i < ParentsCount; i++)
                                   {
                                       var accountBu =
                                           new BusinessUnit
                                           {
                                               Id = Guid.NewGuid(),
                                               Active = true,
                                               Name = StringUtil.UniqueString("Account"),
                                               IsPool = true,
                                               BusinessUnitStatus = BusinessUnitStatus.Current,
                                               IsProduction = true,
                                               BusinessUnitType = accountType,
                                               Period = period
                                           };

                                       context.Logics.Default.Create<BusinessUnit>().Insert(accountBu, accountBu.Id);
                                   }
                               });
        }
    }
}
