using System;
using System.Collections.Generic;


using FluentAssertions;

using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers.Report;

namespace SampleSystem.IntegrationTests
{
    [TestClass]
    public class ReportDefinitionFacadeTests : TestBase
    {
        [TestMethod]
        public void GetStream_ErrorThrownInside_ProcessesError()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.DataHelper.SaveReport();
            this.DataHelper.SaveReportProperty(report, nameof(Employee.PersonalCellPhone));

            var parameter = this.DataHelper.SaveReportParameter(report);
            this.DataHelper.SaveReportFilter(report, parameter);

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
            var action = new Action(() => sampleSystemGenericReportController.Evaluate(c => c.GetStream(model)));

            // Assert
            action.Should().Throw<Exception>();
        }

        [TestMethod]
        public void GetStream_NoFiltersProvided_GenerationSucceedes()
        {
            // Arrange
            var report = this.DataHelper.SaveReport();
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();

            this.DataHelper.SaveReportProperty(report, nameof(Employee.PersonalCellPhone));

            var parameter = this.DataHelper.SaveReportParameter(report);
            this.DataHelper.SaveReportFilter(report, parameter);

            var model = new ReportGenerationModelStrictDTO
            {
                Report = report
            };

            // Act
            sampleSystemGenericReportController.Evaluate(c => c.GetStream(model));
            var action = new Action(() => sampleSystemGenericReportController.Evaluate(c => c.GetStream(model)));

            // Assert
            action.Should().NotThrow();
        }

        [Ignore] // IADFRAME-1501
        [TestMethod]
        public void GetStream_ReportFieldWithMaybe_ReturnsReportWithoutError()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
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
            var action = new Action(() => sampleSystemGenericReportController.Evaluate(c => c.GetStream(model)));

            // Assert
            action.Should().NotThrow();
        }
    }
}
