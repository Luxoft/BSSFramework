using FluentAssertions;

using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Domain.Inline;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class BUProjectionTests : TestBase
    {
        private const string TestEmployee0Login = "Test Employee 0";
        private const string TestEmployee1Login = "Test Employee 1";
        private const string TestEmployee2Login = "Test Employee 2";

        [TestInitialize]
        public void SetUp()
        {
            var buTypeId = this.DataHelper.SaveBusinessUnitType(DefaultConstants.BUSINESS_UNIT_TYPE_COMPANY_ID);

            var luxoftBuId = this.DataHelper.SaveBusinessUnit(
                id: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_ID,
                name: DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME,
                type: buTypeId);

            var costBuId = this.DataHelper.SaveBusinessUnit(
                id: DefaultConstants.BUSINESS_UNIT_PARENT_CC_ID,
                name: DefaultConstants.BUSINESS_UNIT_PARENT_CC_NAME,
                type: buTypeId,
                parent: luxoftBuId);

            var profitBuId = this.DataHelper.SaveBusinessUnit(
                id: DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID,
                name: DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME,
                type: buTypeId,
                parent: luxoftBuId);

            var empId1 = this.DataHelper.SaveEmployee(login: TestEmployee0Login, coreBusinessUnit: costBuId, nameEng: new Fio { FirstName = "AA" });
            var empId2 = this.DataHelper.SaveEmployee(login: TestEmployee1Login, coreBusinessUnit: costBuId, nameEng: new Fio { FirstName = "BB" });
            var empId3 = this.DataHelper.SaveEmployee(login: TestEmployee2Login, coreBusinessUnit: profitBuId, nameEng: new Fio { FirstName = "CC" });

            this.DataHelper.Environment.GetContextEvaluator().Evaluate(
                DBSessionMode.Write,
                context =>
                {
                    var bu = context.Logics.BusinessUnit.GetById(profitBuId.Id, true);

                    foreach (var empId in new[] { empId1, empId2, empId3 })
                    {
                        var emp = context.Logics.Employee.GetById(empId.Id, true);

                        var link = new BusinessUnitEmployeeRole(bu)
                        {
                            Employee = emp
                        };
                    }

                    context.Logics.BusinessUnit.Save(bu);
                });
        }

        [TestMethod]
        public void BusinessUnitProjectionCalcCollectionPropTest()
        {
            // Arrange
            var businessUnitQueryController = this.GetControllerEvaluator<BusinessUnitQueryController>();
            var expectedEmployee = "AA,BB,CC";

            // Act
            var profitBU = businessUnitQueryController.Evaluate(c => c.GetTestBusinessUnitsByODataQueryString($"$filter=Id eq GUID'{DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID}'"));

            // Assert
            profitBU.Items.Should().ContainSingle();
            profitBU.Items[0].Employees.Should().Be(expectedEmployee);
        }

        [TestMethod]
        public void BusinessUnitProjectionCalcHerPropTest()
        {
            // Arrange
            var businessUnitQueryController = this.GetControllerEvaluator<BusinessUnitQueryController>();
            var expectedHer = $"{DefaultConstants.BUSINESS_UNIT_PARENT_PC_NAME},{DefaultConstants.BUSINESS_UNIT_PARENT_COMPANY_NAME}";

            // Act
            var profitBU = businessUnitQueryController.Evaluate(c => c.GetTestBusinessUnitsByODataQueryString($"$filter=Id eq GUID'{DefaultConstants.BUSINESS_UNIT_PARENT_PC_ID}'"));

            // Assert
            profitBU.Items.Should().ContainSingle();
            profitBU.Items[0].HerBusinessUnit_Full.Should().Be(expectedHer);
        }
    }
}
