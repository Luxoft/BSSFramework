using Automation.ServiceEnvironment;
using Automation.Utils;

using Framework.Core;
using Framework.DomainDriven;
using Framework.Persistent;
using SecuritySystem;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.Security;
using SampleSystem.WebApiCore.Controllers.Main;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests;

[TestClass]
public class BusinessUnitTests : TestBase
{
    private const string EmployeeName = "TestSecondaryAccessEmployee";

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

        this.AuthManager.For(EmployeeName).SetRole(
            new SampleSystemTestPermission(
                SampleSystemSecurityRole.TestRole3,
                new BusinessUnitIdentityDTO(DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID)));
    }

    [TestMethod]
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
        businessUnitTree.TotalCount.Should().Be(2);
        businessUnitTree.Items.Count.Should().Be(2);

        businessUnitTree.Items.Should().Contain(node => node.Item.Name == DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME && !node.OnlyView);
        businessUnitTree.Items.Should()
                        .Contain(node => node.Item.Name == DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME && node.OnlyView);
    }

    [TestMethod]
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

    [TestMethod]
    public void LoadTreeWithMiddlePermission_RootParentLoadedWithViewMode()
    {
        // Arrange
        var parentBu = this.DataHelper.SaveBusinessUnit(parentIsNeeded: false);
        var childBu = this.DataHelper.SaveBusinessUnit(parent: parentBu);

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
        result.Count.Should().Be(2);
        result.Should().Contain(node => node.Item == parentBu.Id && node.OnlyView);
        result.Should().Contain(node => node.Item == childBu.Id && !node.OnlyView);
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
