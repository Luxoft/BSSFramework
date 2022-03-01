using System;
using System.Collections.Generic;


using FluentAssertions;

using Framework.Configuration.Generated.DTO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.IntegrationTests.__Support.TestData;
using WorkflowSampleSystem.WebApiCore.Controllers.Report;

namespace WorkflowSampleSystem.IntegrationTests
{
    [TestClass]
    public class ReportDefinitionFacadeTests : TestBase
    {
        [TestMethod]
        public void GetStream_ErrorThrownInside_ProcessesError()
        {
            // Arrange
            var WorkflowSampleSystemGenericReportController = this.GetController<WorkflowSampleSystemGenericReportController>();
            var report = this.DataHelper.SaveReport();
            this.DataHelper.SaveReportProperty(report, nameof(Employee.PersonalCellPhone));

            var parameter = this.DataHelper.SaveReportParameter(report);
            this.DataHelper.SaveReportFilter(report, parameter);

            this.Environment.IsDebugInTest = false;

            var model = new ReportGenerationModelStrictDTO
            {
                Report = report,
                Items = new List<ReportGenerationValueStrictDTO>()
                        {
                            new ReportGenerationValueStrictDTO
                            {
                                    Parameter = parameter,
                                    DesignValue = "wrong data",
                                    Value = "some random not Guid value"
                            }
                        }
            };

            // Act
            var action = new Action(() => WorkflowSampleSystemGenericReportController.GetStream(model));

            // Assert
            this.Environment.IsDebugMode.Should().BeFalse("not working in debugger mode");
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void GetStream_NoFiltersProvided_GenerationSucceedes()
        {
            // Arrange
            var report = this.DataHelper.SaveReport();
            var WorkflowSampleSystemGenericReportController = this.GetController<WorkflowSampleSystemGenericReportController>();

            this.DataHelper.SaveReportProperty(report, nameof(Employee.PersonalCellPhone));

            var parameter = this.DataHelper.SaveReportParameter(report);
            this.DataHelper.SaveReportFilter(report, parameter);

            var model = new ReportGenerationModelStrictDTO
            {
                Report = report
            };

            // Act
            WorkflowSampleSystemGenericReportController.GetStream(model);
            var action = new Action(() => WorkflowSampleSystemGenericReportController.GetStream(model));

            // Assert
            action.Should().NotThrow();
        }

        [Ignore] // IADFRAME-1501
        [TestMethod]
        public void GetStream_ReportFieldWithMaybe_ReturnsReportWithoutError()
        {
            // Arrange
            var WorkflowSampleSystemGenericReportController = this.GetController<WorkflowSampleSystemGenericReportController>();
            var report = this.DataHelper.SaveReport();
            this.DataHelper.SaveReportProperty(report, $"{nameof(Employee.Position)}/{nameof(EmployeePosition.Name)}");

            var parameter = this.DataHelper.SaveReportParameter(report, typeName: nameof(BusinessUnit));
            this.DataHelper.SaveReportFilter(report, parameter);

            var coreBU = this.DataHelper.SaveBusinessUnit();
            this.DataHelper.SaveEmployee(coreBusinessUnit: coreBU);

            var model = new ReportGenerationModelStrictDTO
            {
                Report = report,
                Items = new List<ReportGenerationValueStrictDTO>()
                        {
                            new ReportGenerationValueStrictDTO
                            {
                                Parameter = parameter,
                                DesignValue = "Test BU",
                                Value = coreBU.Id.ToString()
                            }
                        }
            };

            // Act
            var action = new Action(() => WorkflowSampleSystemGenericReportController.GetStream(model));

            // Assert
            action.Should().NotThrow();
        }
    }
}
