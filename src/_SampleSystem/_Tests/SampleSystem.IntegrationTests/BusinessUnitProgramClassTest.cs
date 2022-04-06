using System;
using System.Linq;
using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Main;
using SampleSystem.WebApiCore.Controllers.MainQuery;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class BusinessUnitProgramClassTest : TestBase
    {
        private BusinessUnitIdentityDTO lobFbu;

        [TestInitialize]
        public void TestData()
        {
            var businessUnitTypeController = this.GetControllerEvaluator<BusinessUnitTypeController>();
            var lobType = businessUnitTypeController.Evaluate(c => c.GetSimpleBusinessUnitTypeByName(DefaultConstants.BUSINESS_UNIT_TYPE_LOB_NAME));
            var programType = businessUnitTypeController.Evaluate(c => c.GetSimpleBusinessUnitTypeByName(DefaultConstants.BUSINESS_UNIT_TYPE_PROGRAM_NAME));

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
            var businessUnitQueryController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.MainQuery.BusinessUnitQueryController>();

            var filter = new BusinessUnitProgramClassFilterModelStrictDTO
            {
                AncestorIdent = this.lobFbu.Id,
            };

            // Act
            var actualResult = businessUnitQueryController.Evaluate(c => c.GetBusinessUnitProgramClassesByODataQueryStringWithFilter(new GetBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest
                {
                    odataQueryString = "$top=70&$orderby=VirtualValue",
                    filter = filter
                })).Items;

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
            var businessUnitQueryController = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.MainQuery.BusinessUnitQueryController>();
            var filter = new BusinessUnitProgramClassFilterModelStrictDTO
            {
                FilterVirtualName = namePart,
            };

            // Act
            var actualResult = businessUnitQueryController.Evaluate(c => c.GetBusinessUnitProgramClassesByODataQueryStringWithFilter(new GetBusinessUnitProgramClassesByODataQueryStringWithFilterAutoRequest
                {
                    odataQueryString = "$top=70&$orderby=VirtualValue",
                    filter = filter
                })).Items;

            // Assert
            actualResult.Count.Should().Be(3);

            actualResult.All(z => z.VirtualName.Contains(namePart)).Should().BeTrue();

            var expectedResult = actualResult.OrderBy(z => z.VirtualValue).ToList();

            expectedResult.SequenceEqual(actualResult).Should().BeTrue();
        }
    }
}
