using System;
using System.Linq;

using FluentAssertions;

using Framework.CustomReports.WebApi;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Generated.DTO;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Report;
using SampleSystem.WebApiCore.CustomReports;

namespace SampleSystem.IntegrationTests.Reports
{
    [TestClass]
    public class CustomReportsTests : TestBase
    {
        [TestInitialize]
        public void InitDd()
        {
            ActivatorUtilities.CreateInstance<SampleSystemCustomReportsServiceEnvironment>(this.Environment.ServiceProvider);
        }

        [TestMethod]
        public void GetEmployeeReport_CorrectReportExcel()
        {
            // Arrange
            var reportId = new Guid("7DBDCCDF-1F43-43FA-854A-D465F5F4ED53");

            // Act
            var report = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.CustomReport.EmployeeReportParameterController>().Evaluate(c => c.GetEmployeeReport(new EmployeeReportParameterStrictDTO { ReportId = reportId }));

            // Assert
            report.GetSheet("Parameters")
                .CreateValidator()
                .SetHeaderRowNumber(3)
                .AddSearchCriteria("Value", "7dbdccdf-1f43-43fa-854a-d465f5f4ed53")
                .AddSearchCriteria("Name", "ReportID")
                .ValidateExists();

            report.GetSheet()
                .CreateValidator()
                .SetHeaderRowNumber(1)
                .AddSearchCriteria("Name", "Admin Admin")
                .AddSearchCriteria("Position", "Tester")
                .AddSearchCriteria("Account name", this.DataHelper.GetCurrentEmployee().AccountName)
                .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReport_WithoutAdminRights_CorrectReportExcel()
        {
            // Arrange
            const string Tester = @"luxoft\reporttester1";

            this.AuthHelper.SavePrincipal(Tester, true);

            var reportId = new Guid("7DBDCCDF-1F43-43FA-854A-D465F5F4ED53");

            // Act
            var report = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.CustomReport.EmployeeReportParameterController>(Tester).Evaluate(c => c.GetEmployeeReport(new EmployeeReportParameterStrictDTO { ReportId = reportId }));

            // Assert
            report.GetSheet("Parameters")
                  .CreateValidator()
                  .SetHeaderRowNumber(3)
                  .AddSearchCriteria("Name", "ReportID")
                  .AddSearchCriteria("Value", "7dbdccdf-1f43-43fa-854a-d465f5f4ed53")
                  .ValidateExists();

            report.GetSheet()
                .CreateValidator()
                .SetHeaderRowNumber(1)
                .AddSearchCriteria("Name", "Admin Admin")
                .AddSearchCriteria("Position", "Tester")
                .AddSearchCriteria("Account name", this.DataHelper.GetCurrentEmployee().AccountName)
                .ValidateNotExists();
        }

        [TestMethod]
        public void GetSimpleReportParameterValues_AllValuesReturned()
        {
            // Arrange
            var reportId = new Guid("7DBDCCDF-1F43-43FA-854A-D465F5F4ED53");

            var parameters = this.GetControllerEvaluator<SampleSystemGenericReportController>().Evaluate(c => c.GetSimpleReportParameters(reportId));

            // Act
            var values = this.GetControllerEvaluator<SampleSystemGenericReportController>().Evaluate(c => c.GetSimpleReportParameterValues(new GetSimpleReportParameterValuesRequest
            {
                    identity = parameters.First(x => x.Name.Equals("Position", StringComparison.InvariantCultureIgnoreCase)).Identity,
                    odataQueryString = string.Empty
            }));

            // Assert
            values.TotalCount.Should().Be(1);
            values.Items.First().DesignValue.Should().Be("Tester");
        }

        [TestMethod]
        public void GetSimpleReportParameterValues_SecurityTest_NoValuesReturned()
        {
            // Arrange
            const string Tester = @"luxoft\reporttester";

            this.AuthHelper.SavePrincipal(Tester, true);

            this.AuthHelper.LoginAs(Tester);

            var reportId = new Guid("7DBDCCDF-1F43-43FA-854A-D465F5F4ED53");

            var parameters = this.GetControllerEvaluator<SampleSystemGenericReportController>().Evaluate(c => c.GetSimpleReportParameters(reportId));

            // Act
            var values = this.GetControllerEvaluator<SampleSystemGenericReportController>().Evaluate(c => c.GetSimpleReportParameterValues(new GetSimpleReportParameterValuesRequest
            {
                    identity = parameters.First(x => x.Name.Equals("Position", StringComparison.InvariantCultureIgnoreCase)).Identity,
                    odataQueryString = string.Empty
            }));

            // Assert
            values.TotalCount.Should().Be(0);
        }

        [TestMethod]
        public void GetEmployeeReport_RequestWithOptionalReportParameter_CorrectReportExcel()
        {
            // Arrange
            var reportId = new Guid("7DBDCCDF-1F43-43FA-854A-D465F5F4ED53");

            var parameters = this.GetControllerEvaluator<SampleSystemGenericReportController>().Evaluate(c => c.GetSimpleReportParameters(reportId));
            var values = this.GetControllerEvaluator<SampleSystemGenericReportController>().Evaluate(c => c.GetSimpleReportParameterValues(new GetSimpleReportParameterValuesRequest
            {
                identity = parameters.First(x => x.Name.Equals("Position", StringComparison.InvariantCultureIgnoreCase)).Identity,
                odataQueryString = string.Empty
            }));

            var positionId = new Guid(values.Items.First().Value);

            // Act
            var report = this.GetControllerEvaluator<SampleSystem.WebApiCore.Controllers.CustomReport.EmployeeReportParameterController>()
                             .Evaluate(c => c.GetEmployeeReport(new EmployeeReportParameterStrictDTO { ReportId = reportId, Position = new EmployeePositionIdentityDTO(positionId) }));

            // Assert
            report.GetSheet("Parameters")
                  .CreateValidator()
                  .SetHeaderRowNumber(3)
                  .AddSearchCriteria("Name", "ReportID")
                  .AddSearchCriteria("Value", "7dbdccdf-1f43-43fa-854a-d465f5f4ed53")
                  .ValidateExists();

            report.GetSheet()
                  .CreateValidator()
                  .SetHeaderRowNumber(1)
                  .AddSearchCriteria("Name", "Admin Admin")
                  .AddSearchCriteria("Position", "Tester")
                  .AddSearchCriteria("Account name", this.DataHelper.GetCurrentEmployee().AccountName)
                  .ValidateExists();
        }
    }
}
