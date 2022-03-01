using System;
using System.Linq;
using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkflowSampleSystem.Generated.DTO;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.WebApiCore.Controllers.Main;
using WorkflowSampleSystem.WebApiCore.Controllers.MainQuery;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class BusinessUnitProgramClassTest : TestBase
    {
        private BusinessUnitIdentityDTO lobFbu;

        [TestInitialize]
        public void TestData()
        {
            var businessUnitTypeController = this.GetController<BusinessUnitTypeController>();
            var lobType = businessUnitTypeController.GetSimpleBusinessUnitTypeByName(DefaultConstants.BUSINESS_UNIT_TYPE_LOB_NAME);
            var programType = businessUnitTypeController.GetSimpleBusinessUnitTypeByName(DefaultConstants.BUSINESS_UNIT_TYPE_PROGRAM_NAME);

            var profitCenter = this.DataHelper.SaveBusinessUnit(type: programType.Identity, name: "ProfitCenter"); ;

            this.DataHelper.SaveBusinessUnit(type: programType.Identity, parent: profitCenter, name: "100gramm");

            this.lobFbu = this.DataHelper.SaveBusinessUnit(type: lobType.Identity, parent: profitCenter, name: "LOB");
            this.DataHelper.SaveBusinessUnit(type: programType.Identity, parent: this.lobFbu, name: "Programm");
            this.DataHelper.SaveBusinessUnit(type: programType.Identity, parent: this.lobFbu, name: "Telegramm");
        }

        [TestMethod]
        public void Get_SortByVirtualProperty_CheckOrderSuccessed()
        {
            // Arrange
            var businessUnitQueryController = this.GetController<WorkflowSampleSystem.WebApiCore.Controllers.MainQuery.BusinessUnitQueryController>();

            var filter = new BusinessUnitProgramClassFilterModelStrictDTO
            {
                AncestorIdent = this.lobFbu.Id,
            };

            // Act
            var actualResult = businessUnitQueryController.GetBusinessUnitProgramClassesByODataQueryStringWithFilter(new GetBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest
                {
                    odataQueryString = "$top=70&$orderby=VirtualValue",
                    filter = filter
                }).Items;

            // Assert

            actualResult.Count.Should().Be(3);

            var expectedResult = actualResult.OrderBy(z => z.VirtualValue).ToList();

            expectedResult.SequenceEqual(actualResult).Should().BeTrue();
        }

        [TestMethod]
        public void Get_FilterByVirtualProperty_CheckFilterSuccessed()
        {
            // Arrange
            var namePart = "gramm";
            var businessUnitQueryController = this.GetController<WorkflowSampleSystem.WebApiCore.Controllers.MainQuery.BusinessUnitQueryController>();
            var filter = new BusinessUnitProgramClassFilterModelStrictDTO
            {
                FilterVirtualName = namePart,
            };

            // Act
            var actualResult = businessUnitQueryController.GetBusinessUnitProgramClassesByODataQueryStringWithFilter(new GetBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest
                {
                    odataQueryString = "$top=70&$orderby=VirtualValue",
                    filter = filter
                }).Items;

            // Assert
            actualResult.Count.Should().Be(3);

            actualResult.All(z => z.VirtualName.Contains(namePart)).Should().BeTrue();

            var expectedResult = actualResult.OrderBy(z => z.VirtualValue).ToList();

            expectedResult.SequenceEqual(actualResult).Should().BeTrue();
        }
    }
}
