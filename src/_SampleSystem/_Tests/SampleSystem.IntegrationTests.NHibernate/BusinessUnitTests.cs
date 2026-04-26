using Framework.Application;
using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.AutomationCore.Utils;
using Framework.BLL.Domain.Persistent;
using Framework.Core;
using Framework.Database;

using Anch.SecuritySystem;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Enums;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.MainQuery;

using BusinessUnitController = SampleSystem.WebApiCore.Controllers.Main.BusinessUnitController;

namespace SampleSystem.IntegrationTests;

public class BusinessUnitTests : TestBase
{
    private const string EmployeeName = "TestSecondaryAccessEmployee";

    private const int ParentsCount = 2200;

    private const int ExistedBusinessUnitsInDatabase = 3;

    public BusinessUnitTests()
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

        this.AuthManager.For(EmployeeName).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.TestRole3,
                new BusinessUnitIdentityDTO(DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID)));
    }

    [Fact]
    public void CheckBusinessUnitSecondaryAccess_HasAccess()
    {
        // Arrange
        var businessUnitQueryController = this.GetControllerEvaluator<BusinessUnitQueryController>(EmployeeName);

        // Act
        var businessUnitTree = businessUnitQueryController.Evaluate(
            c => c.GetTestBusinessUnitTreeByOperation(
                new GetTestBusinessUnitTreeByOperationAutoRequest
                {
                    OdataQueryString = string.Empty,
                    SecurityRule = new DomainSecurityRule.ClientSecurityRule(SampleSystemSecurityOperation.EmployeeEdit.Name)
                }));

        // Assert
        Assert.Equal(2, businessUnitTree.TotalCount);
        Assert.Equal(2, businessUnitTree.Items.Length);

        Assert.Contains(businessUnitTree.Items, node => node.Item.Name == DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME && !node.OnlyView);
        Assert.Contains(businessUnitTree.Items, node => node.Item.Name == DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME && node.OnlyView);
    }

    [Fact]
    public void GetTreeWithFilter()
    {
        // Arrange
        var businessUnitQueryController = this.GetControllerEvaluator<BusinessUnitQueryController>(EmployeeName);

        // Act
        var businessUnitTree = businessUnitQueryController.Evaluate(
            c => c.GetTestBusinessUnitTreeByOperation(
                new GetTestBusinessUnitTreeByOperationAutoRequest
                {
                    OdataQueryString = "$filter=Name eq 'test'",
                    SecurityRule = new DomainSecurityRule.ClientSecurityRule(SampleSystemSecurityOperation.EmployeeEdit.Name)
                }));

        // Assert
        Assert.Equal(0, businessUnitTree.TotalCount);
        Assert.Empty(businessUnitTree.Items);
    }

    // Two test in one for performance reasons
    [Fact]
    public void GetFullBusinessUnitsTreeTwoTestInOne()
    {
        // Arrange
        this.CreateBigBuTree();

        this.TestGetFullBusinessUnitsTreeByOData();

        this.TestGetFullBusinessUnitsTree();
    }

    [Fact]
    public void LoadTreeWithMiddlePermission_RootParentLoadedWithViewMode()
    {
        // Arrange
        var parentBu = this.DataHelper.SaveBusinessUnit(parentIsNeeded: false);
        var childBu = this.DataHelper.SaveBusinessUnit(parent: parentBu);

        this.ClearIntegrationEvents();

        var userId = this.AuthManager.For(TextRandomizer.RandomString(10))
                         .SetRole(new SampleSystemTestPermission(SampleSystemSecurityRole.SeManager, childBu));

        // Act
        var result = this.Evaluate(
            DBSessionMode.Read,
            userId,
            ctx =>
            {
                var buTree = ctx.Logics.BusinessUnitFactory.Create(SecurityRule.Edit).GetTree();

                return buTree.ChangeItem(bu => bu.Id);
            });

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, node => node.Item == parentBu.Id && node.OnlyView);
        Assert.Contains(result, node => node.Item == childBu.Id && !node.OnlyView);
    }

    private void TestGetFullBusinessUnitsTreeByOData()
    {
        // Act
        var businessUnitQueryController = this.GetControllerEvaluator<BusinessUnitController>();
        var businessUnitTree = businessUnitQueryController.Evaluate(c => c.GetFullBusinessUnitsTreeByOData(string.Empty));

        // Assert
        Assert.Equal(ParentsCount + ExistedBusinessUnitsInDatabase, businessUnitTree.TotalCount);
        Assert.Equal(ParentsCount + ExistedBusinessUnitsInDatabase, businessUnitTree.Items.Length);
    }

    private void TestGetFullBusinessUnitsTree()
    {
        // Act
        var businessUnitController = this.GetControllerEvaluator<BusinessUnitController>();
        var tree = businessUnitController.Evaluate(c => c.GetFullBusinessUnitsTree());

        // Assert
        Assert.Equal(ParentsCount + ExistedBusinessUnitsInDatabase, tree.Count());
    }

    private void CreateBigBuTree()
    {
        var buAccountId = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_ACCOUNT_ID);

        this.DataHelper
            .EvaluateWrite(
                context =>
                {
                    var period = new Period(
                        this.FinancialYearService.GetCurrentFinancialYear().StartDate
                            .AddYears(-1));
                    var accountType = context.Logics.BusinessUnitType.GetById(buAccountId.Id);

                    for (var i = 0; i < ParentsCount; i++)
                    {
                        var accountBu =
                            new BusinessUnit
                            {
                                Id = Guid.NewGuid(),
                                Active = true,
                                Name = TextRandomizer.UniqueString("Account"),
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
