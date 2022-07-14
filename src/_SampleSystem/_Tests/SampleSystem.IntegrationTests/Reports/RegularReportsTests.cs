// ReSharper disable ObjectCreationAsStatement

using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Framework.Authorization.Generated.DTO;
using Framework.Configuration.Domain.Reports;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.CustomReports.Services.ExcelBuilder;
using Framework.CustomReports.WebApi;
using Framework.DomainDriven.BLL;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.Domain;
using SampleSystem.Domain.Enums;
using SampleSystem.Domain.Inline;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore.Controllers;
using SampleSystem.WebApiCore.Controllers.Report;

namespace SampleSystem.IntegrationTests.Reports
{
    [TestClass]
    public class RegularReportsTests : TestBase
    {
        [TestInitialize]
        public void Setup()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("ru-ru");
        }


        [TestMethod]
        public void GetReport_GetLocationNameReportGetDepartmentNameReport_CorrectBuild()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();

            var location = this.DataHelper.SaveLocation(name: "loc1");
            var department = this.DataHelper.SaveHRDepartment(name: "dep");

            var locationReport2Identity = this.CreateLocationNameReport();
            var locationReport1Identity = this.CreateLocationNameReport();

            var departmentReportReport = this.CreateReport<HRDepartment>("HRDepartmentReport");
            AppendReportProperty(departmentReportReport, "Name", "Name");
            var departmentReportIdentity = this.SaveReport(departmentReportReport);


            var locationReportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(new ReportGenerationModelStrictDTO() { Report = locationReport1Identity }));
            var departmentReportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(new ReportGenerationModelStrictDTO() { Report = departmentReportIdentity }));
            var location2ReportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(new ReportGenerationModelStrictDTO() { Report = locationReport2Identity }));


            Assert.IsNotNull(locationReportStream);
            Assert.IsNotNull(departmentReportStream);
        }

        private ReportIdentityDTO CreateLocationNameReport()
        {
            var location2Report = this.CreateReport<Location>("LocationReport" + Guid.NewGuid().ToString());
            AppendReportProperty(location2Report, "Name", "Name");
            var locationReport2Identity = this.SaveReport(location2Report);
            return locationReport2Identity;
        }

        [TestMethod]
        public void GetEmployeeReport_ReportForPersistentPropertiesWithVirtualFilterNextChangeProperty_CorrectBuild()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();

            var location = this.DataHelper.SaveLocation(name: "loc1");

            this.DataHelper.SaveLocation(name: "loc2");

            this.DataHelper.SaveEmployee(location: location, hrDepartment: this.DataHelper.SaveHRDepartment(codeNative: "CodeNative", code: "Code"));

            var report = this.CreateReport<Employee>("EmployeeReport");

            AppendReportProperty(report, "HRDepartment.Code", "hrDepartmentVisualIdentity");

            AppendReportProperty(report, "NameEng.FirstName", "name");

            AppendReportFilter(report, "Location", "eq", Guid.NewGuid().ToString(), false);

            var reportRichDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportRichDTO, new Dictionary<string, object>());

            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);

            // Act
            var hrDepartmentProperty = reportRichDTO.Properties.Single(z => z.PropertyPath == "HRDepartment.Code");

            hrDepartmentProperty.PropertyPath = "HRDepartment.CodeNative";

            sampleSystemGenericReportController.Evaluate(c => c.SaveReport(reportRichDTO.ToStrict()));

            reportRichDTO = sampleSystemGenericReportController.Evaluate(c => c.GetRichReport(reportRichDTO.Identity));

            reportGenerationModel = CreateReportModel(reportRichDTO, new Dictionary<string, object>());

            // Assert
            reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);
        }

        [TestMethod]
        public void GetEmployeeReport_ReportForPersistentPropertiesWithVirtualFilterNextChangePropertyNextChangeProperty_CorrectBuild()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var location = this.DataHelper.SaveLocation(name: "loc1");

            this.DataHelper.SaveLocation(name: "loc2");

            this.DataHelper.SaveEmployee(location: location, hrDepartment: this.DataHelper.SaveHRDepartment(codeNative: "CodeNative", code: "Code"));

            var report = this.CreateReport<Employee>("EmployeeReport");

            AppendReportProperty(report, "HRDepartment.Code", "hrDepartmentVisualIdentity");

            AppendReportProperty(report, "NameEng.FirstName", "name");

            AppendReportFilter(report, "Location", "eq", Guid.NewGuid().ToString(), false);

            var reportRichDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportRichDTO, new Dictionary<string, object>());

            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);

            // Act
            var hrDepartmentProperty = reportRichDTO.Properties.Single(z => z.PropertyPath == "HRDepartment.Code");

            hrDepartmentProperty.PropertyPath = "HRDepartment.CodeNative";

            sampleSystemGenericReportController.Evaluate(c => c.SaveReport(reportRichDTO.ToStrict()));


            var reportRichDTO1 = this.GetReport(new ReportIdentityDTO(report.Id));
            var hrDepartmentProperty2 = reportRichDTO1.Properties.Single(z => z.PropertyPath == "HRDepartment.CodeNative");
            hrDepartmentProperty2.PropertyPath = "HRDepartment.Code";
            sampleSystemGenericReportController.Evaluate(c => c.SaveReport(reportRichDTO1.ToStrict()));


            reportRichDTO = sampleSystemGenericReportController.Evaluate(c => c.GetRichReport(reportRichDTO.Identity));

            reportGenerationModel = CreateReportModel(reportRichDTO, new Dictionary<string, object>());

            // Assert
            reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);
        }


        [TestMethod]
        public void GetEmployeeReport_ReportForPersistentPropertiesWithVirtualFilter_CorrectBuild()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            this.DataHelper.SaveLocation(name: "loc1");
            this.DataHelper.SaveLocation(name: "loc2");

            var report = this.CreateReport<Employee>("EmployeeReport");
            AppendReportProperty(report, "NameEng.FirstName", "name");
            AppendReportFilter(report, "Location", "eq", Guid.NewGuid().ToString(), false);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Assert
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);
        }

        [TestMethod]
        public void GetLocationReport_ReportForPersistentPropertiesWithStringFilter_CorrectBuild()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            this.DataHelper.SaveLocation(name: "loc1");
            this.DataHelper.SaveLocation(name: "loc2");

            var report = this.CreateReport<Location>("LocationReport");
            AppendReportProperty(report, "Name", "name");
            AppendReportFilter(report, "Name", "eq", "loc1", false);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Assert
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);
        }

        [TestMethod]
        public void GetHRDepartmentReport_ReportForPersistentPropertiesWithLocaionFilter_CorrectBuild()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var loc1 = this.DataHelper.SaveLocation(name: "loc1");
            var loc2 = this.DataHelper.SaveLocation(name: "loc2");

            this.DataHelper.SaveHRDepartment(name: "dep1", location: loc1);
            this.DataHelper.SaveHRDepartment(name: "dep2", location: loc2);

            var report = this.CreateReport<HRDepartment>("HRDepartmentPersistentProperties");
            AppendReportProperty(report, "Name", "name");
            AppendReportFilter(report, "Location", "eq", loc1.Id.ToString(), false);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Assert
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);
        }

        [TestMethod]
        public void GetEmployeeReport_ReportForPersistentProperties_CorrectOrder()
        {
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "abc" }, nameNative: new Fio() { FirstName = "a" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "abc" }, nameNative: new Fio() { FirstName = "z" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "zzz" }, nameNative: new Fio() { FirstName = "ab" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "abcf" }, nameNative: new Fio() { FirstName = "b" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));

            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendOrderedReportProperty(report, "NameEng.FirstName", "NameEng", 1, 1, 0);
            AppendOrderedReportProperty(report, "NameNative.FirstName", "NameNative", 2, 1, 1);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            var actual = new List<Tuple<string, string>>();

            var sheet = reportStream.GetSheet();

            for (int row = 3; row <= sheet.Dimension.Rows; row++)
            {
                actual.Add(Tuple.Create(sheet.Cells[row, 1].Value.ToString(), sheet.Cells[row, 2].Value.ToString()));
            }

            var expected = actual.OrderBy(z => z.Item1).ThenBy(z => z.Item2).ToList();

            // Assert
            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void GetEmployeeReport_ReportForPersistentVirtualProperties_CorrectOrder()
        {
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "abc" }, nameNative: new Fio() { FirstName = "a" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "abc" }, nameNative: new Fio() { FirstName = "z" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "zzz" }, nameNative: new Fio() { FirstName = "ab" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));
            this.DataHelper.SaveEmployee(nameEng: new Fio() { FirstName = "abcf" }, nameNative: new Fio() { FirstName = "b" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));

            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendReportProperty(report, "LogonName", "logonName");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.AreEqual(reportStream.GetSheet().Dimension.Columns, 1);

            // Act

            AppendReportProperty(report, "IsCandidate", "IsCandidate");

            reportDTO = this.GetReport(this.SaveReport(report));

            reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            var sheet = reportStream.GetSheet();

            Assert.AreEqual(sheet.Dimension.Columns, 2);
        }

        [TestMethod]
        public void GetEmployeeReport_ReportForPersistentProperties_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendReportProperty(report, "BirthDate", "Birth Date");
            AppendReportProperty(report, "NameEng", "Name");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                  .CreateValidator()
                  .SetHeaderRowNumber(1)
                  .AddSearchCriteria("Name", "Admin Admin")
                  .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReport_ReprotWithBoolProperty_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();

            this.DataHelper.SaveEmployee(active: true, nameEng: new Fio() { FirstName = "abc" }, nameNative: new Fio() { FirstName = "a" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));
            this.DataHelper.SaveEmployee(active: false, nameEng: new Fio() { FirstName = "ac" }, nameNative: new Fio() { FirstName = "a" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));

            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendReportProperty(report, "Active", "Active");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            var sheet = reportStream.GetSheet();

            reportStream.GetSheet().CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Active", "True")
                        .ValidateExists();

            reportStream.GetSheet().CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Active", "False")
                        .ValidateExists();
        }

        [TestMethod]
        public void GetProjectReport_PersistentPropertyWithEndDate_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var startDate = new DateTime(2021, 4, 1);
            var endDate = new DateTime(2021, 4, 10);

            this.DataHelper.SaveBusinessUnit(period: new Period(startDate, endDate));

            var report = this.CreateReport<BusinessUnit>("BusinessUnitReport");
            AppendReportProperty(report, "Period/EndDate", "EndDate");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            var excelWorksheet = reportStream.GetSheet();

            var endDateCell1 = excelWorksheet.Cells[2, 1];
            endDateCell1.Text.Should().Be(endDate.ToString(DefaultExcelCellFormat.GetDefaultFormat(typeof(DateTime))));
        }

        [TestMethod]
        public void GetProjectReport_VirtualPropertyWithEndDate_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var startDate = new DateTime(2021, 4, 1);
            var endDate = new DateTime(2021, 4, 10);

            this.DataHelper.SaveBusinessUnit(period: new Period(startDate, endDate));

            var report = this.CreateReport<BusinessUnit>("BusinessUnitReport");
            AppendReportProperty(report, "Period/EndDate", "EndDate", 1);
            AppendReportProperty(report, "IsSpecialCommission", "IsSpecialCommission", 2);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            var excelWorksheet = reportStream.GetSheet();
            var endDateCell1 = excelWorksheet.Cells[2, 1];
            endDateCell1.Text.Should().Be(endDate.ToString(DefaultExcelCellFormat.GetDefaultFormat(typeof(DateTime))));
        }


        [TestMethod]
        public void GetEmployeeReport_FilterByEnumParameter_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendReportProperty(report, "BirthDate", "Birth Date");
            AppendReportProperty(report, "NameEng", "Name");
            AppendReportParameter<Gender>(report, "Gender");
            AppendReportFilter(report, "Gender", "eq", "Gender");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>() { { "Gender", "0" } });

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            var sheet = reportStream.GetSheet("Parameters");

            // Assert
            Assert.AreEqual("Gender", sheet.Cells[4, 1].Value);
            Assert.AreEqual("0", sheet.Cells[4, 2].Value);
        }

        [TestMethod]
        public void GetEmployeeReport_ReportForVirtualProperties_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();

            var report = this.CreateReport<Employee>("EmployeeVirtualProperties");
            AppendReportProperty(report, "AccountName", "Account name");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Account name", this.DataHelper.GetCurrentEmployee().AccountName)
                        .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReport_ValueParameter_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var birthDate = DateTime.Now.Date;

            this.DataHelper.SaveEmployee(
                login: "john@luxoft.com",
                nameEng: new Fio { FirstName = "John", LastName = "Doe" },
                birthDate: DateTime.Now.Date);

            var report = this.CreateReport<Employee>("EmployeeSimpleParameter");
            AppendReportParameter<DateTime?>(report, "BirthDate");
            AppendReportFilter(report, "BirthDate", "eq");
            AppendReportProperty(report, "BirthDate", "Birth Date");
            AppendReportProperty(report, "NameEng", "Name");
            var reportDTO = this.GetReport(this.SaveReport(report));

            var parameters = new Dictionary<string, object>
            {
                { "BirthDate", birthDate }
            };

            var reportGenerationModel = CreateReportModel(reportDTO, parameters);

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Name", "Doe John")
                        .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReport_PeriodParameter_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var workPeriod = new Period(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date);

            this.DataHelper.SaveEmployee(
                login: "john@luxoft.com",
                nameEng: new Fio { FirstName = "John", LastName = "Doe" },
                birthDate: DateTime.Now.Date,
                workPeriod: workPeriod);

            var report = this.CreateReport<Employee>("EmployeeSimpleParameter");
            AppendReportParameter<Period>(report, "WorkPeriod");
            AppendReportFilter(report, "WorkPeriod", "eq");
            AppendReportProperty(report, "WorkPeriod", "Work Period");
            AppendReportProperty(report, "NameEng", "Name");
            var reportDTO = this.GetReport(this.SaveReport(report));

            var parameters = new Dictionary<string, object>
            {
                { "WorkPeriod", workPeriod }
            };

            var reportGenerationModel = CreateReportModel(reportDTO, parameters);

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Name", "Doe John")
                        .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReport_ReferenceParameter_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var locationId = this.DataHelper.SaveLocation(name: "Moscow");

            var hrDepartmentId = this.DataHelper.SaveHRDepartment(name: "Department", location: locationId);

            this.DataHelper.SaveEmployee(
                login: "john@luxoft.com",
                nameEng: new Fio { FirstName = "John", LastName = "Doe" },
                birthDate: DateTime.Now.Date,
                hrDepartment: hrDepartmentId);

            var report = this.CreateReport<Employee>("EmployeeSimpleParameter");
            AppendReportParameter<Location>(report, "HRDepartment/Location");
            AppendReportFilter(report, "HRDepartment/Location", "eq");
            AppendReportProperty(report, "HRDepartment/Location/Name", "Location");
            AppendReportProperty(report, "NameEng", "Name");
            var reportDTO = this.GetReport(this.SaveReport(report));

            var parameters = new Dictionary<string, object>
            {
                { "HRDepartment/Location", locationId }
            };

            var reportGenerationModel = CreateReportModel(reportDTO, parameters);

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Name", "Doe John")
                        .AddSearchCriteria("Location", "Moscow")
                        .ValidateExists();
        }

        /// <summary>
        /// IADFRAME-1601 Bad request при построении отчета с параметром Company Legal Entity
        /// </summary>
        [TestMethod]
        public void GetEmployeeReport_ParameterThatContainsPropertyWithTheSameType_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var legalEntityId = this.DataHelper.SaveCompanyLegalEntity(name: "Luxoft Professional");
            var locationId = this.DataHelper.SaveLocation(name: "Moscow");

            var hrDepartmentId = this.DataHelper.SaveHRDepartment(name: "Department", location: locationId, companyLegalEntity: legalEntityId);

            this.DataHelper.SaveEmployee(
                login: "john@luxoft.com",
                nameEng: new Fio { FirstName = "John", LastName = "Doe" },
                birthDate: DateTime.Now.Date,
                hrDepartment: hrDepartmentId);

            var report = this.CreateReport<Employee>("EmployeeSimpleParameter");
            AppendReportParameter<Location>(report, "HRDepartment/Location");
            AppendReportParameter<Location>(report, "HRDepartment/CompanyLegalEntity");

            AppendReportFilter(report, "HRDepartment/Location", "eq");
            AppendReportFilter(report, "HRDepartment/CompanyLegalEntity", "eq");

            AppendReportProperty(report, "HRDepartment/Location/Name", "Location");
            AppendReportProperty(report, "NameEng/FullName", "Name");
            var reportDTO = this.GetReport(this.SaveReport(report));

            var parameters = new Dictionary<string, object>
            {
                { "HRDepartment/CompanyLegalEntity", legalEntityId }
            };

            var reportGenerationModel = CreateReportModel(reportDTO, parameters);

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Name", "Doe John")
                        .AddSearchCriteria("Location", "Moscow")
                        .ValidateExists();
        }

        [TestMethod]
        public void GetStream_TwoReportForDepartmentWithOtherLocationSubset_Correct()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report1 = this.CreateReport<HRDepartment>("HRDepartmentReport");
            AppendReportProperty(report1, "Name", "Name");
            AppendReportProperty(report1, "Location/Name", "Location/Name");
            AppendReportProperty(report1, "Location/Id", "Location/Id");
            AppendReportProperty(report1, "Location/Active", "Location/Active");

            var report2 = this.CreateReport<HRDepartment>("HRDepartmentReport");
            AppendReportProperty(report2, "Name", "Name");
            AppendReportProperty(report2, "Location/Name", "Location/Name");
            AppendReportProperty(report2, "Location/Id", "Location/Id");
            AppendReportProperty(report2, "Location/Active", "Location/Active");
            AppendReportProperty(report2, "Location/CreateDate", "Location/CreateDate");

            var dto = new[] { report1, report2 }.Select(z => this.GetReport(this.SaveReport(z))).ToList();

            var models = dto.Select(z => CreateReportModel(z));

            // Act
            Action buildReports = () => models.Foreach(z => sampleSystemGenericReportController.Evaluate(c => c.GetStream(z)));

            // Assert
            buildReports.Should().NotThrow();
        }

        [TestMethod]
        public void GetDepartmentTest()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<HRDepartment>("HRDepartmentReport");
            AppendReportProperty(report, "Name", "Name");
            AppendReportProperty(report, "Location/Name", "Location/Name");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO);

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            Assert.IsNotNull(reportStream);
        }

        [TestMethod]
        public void GeneratateReport_FilterPropertyFromParameter_SuccesedGenerate()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var locationIdentity = this.DataHelper.SaveLocation();

            var report = this.CreateReport<HRDepartment>("HRDepartmentReport");
            AppendReportProperty(report, "Name", "Name");
            AppendReportProperty(report, "Location/Name", "Location/Name");

            AppendReportParameter<Location>(report, "loc");
            AppendReportFilter(report, "Location", "eq", "loc");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var parameters = new Dictionary<string, object>();
            parameters.Add(reportDTO.Parameters.First().Id.ToString(), locationIdentity.Id.ToString());

            var generateModel = CreateReportModel(reportDTO, parameters);

            // Act
            var result = sampleSystemGenericReportController.Evaluate(c => c.GetStream(generateModel));

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GeneratateReport_AddFilterPropertyFromParameter_SuccesedGenerate()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var locationIdentity = this.DataHelper.SaveLocation();

            var report = this.CreateReport<HRDepartment>("HRDepartmentReport");
            AppendReportProperty(report, "Name", "Name");
            AppendReportProperty(report, "Location/Name", "Location/Name");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO);

            var prevReportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            AppendReportParameter<Location>(report, "loc");
            AppendReportFilter(report, "Location", "eq", "loc");

            reportDTO = this.GetReport(this.SaveReport(report));

            var parameters = new Dictionary<string, object>();
            parameters.Add(reportDTO.Parameters.First().Id.ToString(), locationIdentity.Id.ToString());

            var nextReportGenerationModel = CreateReportModel(reportDTO, parameters);

            // Act
            var nextReportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(nextReportGenerationModel));

            // Assert
            Assert.IsNotNull(nextReportStream);
            Assert.IsNotNull(prevReportStream);
        }

        [TestMethod]
        public void GetEmployeeReport_ReportWithSecurityProperty_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            this.DataHelper.SaveEmployee(
                login: "john@luxoft.com", personalCellPhone: "1234567");

            var report = this.CreateReport<Employee>("EmployeeSimpleParameter");
            AppendReportProperty(report, "Login", "Login");
            AppendReportProperty(report, "PersonalCellPhone", "Cell Phone");
            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Login", "john@luxoft.com")
                        .AddSearchCriteria("Cell Phone", "1234567")
                        .ValidateExists();
        }

        [TestMethod]
        public void GetReportForUnSecuredDomainType_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var entityId = this.DataHelper.SaveCompanyLegalEntity(Guid.NewGuid());

            this.EvaluateWrite(context =>
            {
                var entity = context.Logics.CompanyLegalEntity.GetById(entityId.Id);
                var country = context.Logics.Country.GetObjectBy(c => c.Code == "RU");

                new Address(entity)
                {
                    CountryName = country,
                    CityName = "Moscow"
                };

                context.Logics.CompanyLegalEntity.Save(entity);
            });

            var report = this.CreateReport<Address>("Addresses");
            AppendReportProperty(report, "CityName", "City");
            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("City", "Moscow")
                        .ValidateExists();
        }

        [TestMethod]
        public void GetSimpleReportParameterValuesByTypeName_Correct()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var expectedLocationName = "Test Location";

            this.DataHelper.SaveLocation(name: expectedLocationName);

            var results = sampleSystemGenericReportController.Evaluate(c => c.GetSimpleReportParameterValuesByTypeName(new GetSimpleReportParameterValuesByTypeNameRequest
            {
                    typeName = "Location",
                    odataQueryString = "$select=Id, DesignValue&$top=80&$orderby=DesignValue"
            }));

            Assert.IsTrue(results.Items.Any(z => z.DesignValue == expectedLocationName));
        }

        [TestMethod]
        public void GetEmployeeReport_ContextualParameter_CorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var businessUnitIdentity = this.DataHelper.SaveBusinessUnit(name: "Test BusinessUnit");

            this.DataHelper.SaveEmployee(
                login: "john@luxoft.com",
                nameEng: new Fio { FirstName = "John", LastName = "Doe" },
                coreBusinessUnit: businessUnitIdentity);

            var report = this.CreateReport<Employee>("EmployeeСontextualParameter");

            AppendReportParameter<Location>(report, "CoreBusinessUnit");
            AppendReportFilter(report, "CoreBusinessUnit", "eq");

            AppendReportProperty(report, "NameEng", "Name");
            AppendReportProperty(report, "CoreBusinessUnit", "BusinessUnit");

            var reportDTO = this.GetReport(this.SaveReport(report));

            var parameters = new Dictionary<string, object>
            {
                { "CoreBusinessUnit", businessUnitIdentity }
            };

            var reportGenerationModel = CreateReportModel(reportDTO, parameters);

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Name", "Doe John")
                        .AddSearchCriteria("BusinessUnit", "Test BusinessUnit")
                        .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReport_PrincipalWithRole_CorrectReportExcel()
        {
            const string Tester = @"luxoft\reporttester";

            var principalIdentity = this.AuthHelper.SavePrincipal(Tester, true);

            var businessRole = this.GetAuthControllerEvaluator().Evaluate(c => c.GetRichBusinessRoleByName("SecretariatNotification"));

            var operation = this.GetAuthControllerEvaluator().Evaluate(c => c.GetSimpleOperationByName(SampleSystemSecurityOperationCode.EmployeeView.ToString()));

            var permission = new PermissionStrictDTO { Role = businessRole.Identity };

            var saveRequest = new AuthSLJsonController.SavePermissionAutoRequest(principalIdentity, permission);
            this.GetAuthControllerEvaluator().Evaluate(c => c.SavePermission(saveRequest));

            var report = this.CreateReport<Employee>("Employees");
            AppendReportProperty(report, "Login", "Login");

            var reportDTO = this.GetReport(this.SaveReport(report));
            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = this.GetControllerEvaluator<SampleSystemGenericReportController>(Tester).Evaluate(c => c.GetStream(reportGenerationModel));

            // проверяем, что отчет пустой,
            // поскольку прав просмотр на сотрудника, а именно на поле Login,
            // у принципала luxoft\reporttester нет.
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Login", Tester)
                        .ValidateNotExists();

            // добавляем право на просмотр сотрудника

            businessRole.BusinessRoleOperationLinks.Add(new BusinessRoleOperationLinkRichDTO
            {
                BusinessRole = businessRole,
                Operation = operation,
            });

            this.GetAuthControllerEvaluator().Evaluate(c => c.SaveBusinessRole(businessRole.ToStrict()));

            reportStream = this.GetControllerEvaluator<SampleSystemGenericReportController>(Tester).Evaluate(c => c.GetStream(reportGenerationModel));

            // проверяем, что отчет содержит ожидаемые данные,
            // поскольку право на просмотр на сотрудника, а именно на поле Login,
            // у принципала luxoft\reporttester теперь есть.
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria("Login", this.DataHelper.GetCurrentEmployee().Login.GetValue())
                        .ValidateExists();
        }

        /// <summary>
        /// IADFRAME-1592 В отчетах по доменным объектам, данные содержащие числа, интерпретируются как строки
        /// </summary>
        [TestMethod]
        public void GetEmployeeReport_ReportForNumberProperties_CorrectTypeAndValue()
        {
            this.DataHelper.SaveEmployee(pin: 10, nameEng: new Fio { FirstName = "abc" }, nameNative: new Fio { FirstName = "a" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));

            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendOrderedReportProperty(report, "NameEng.FirstName", "NameEng", 1, 1, 0);
            AppendOrderedReportProperty(report, "NameNative.FirstName", "NameNative", 2, 1, 1);
            AppendOrderedReportProperty(report, "Pin", "Pin", 3, 1, 2);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            var sheet = reportStream.GetSheet();

            var row = 2;
            var nameEng = sheet.Cells[row, 1].Value;
            var nameNative = sheet.Cells[row, 2].Value;
            var pin = sheet.Cells[row, 3].Value;

            nameEng.GetType().Should().Be(typeof(string));
            nameNative.GetType().Should().Be(typeof(string));
            pin.GetType().Should().Be(typeof(double)); // All numbers converted to double by Excel
            pin.Should().Be(10);
        }

        /// <summary>
        /// IADFRAME-1599 Не строится отчет, если в настройках отчета заполнено поле Formula
        /// </summary>
        [TestMethod]
        public void GetEmployeeReport_ReportWithHyperlinkFormula_CorrectTypeAndValue()
        {
            this.DataHelper.SaveEmployee(pin: 10, nameEng: new Fio { FirstName = "abc" }, nameNative: new Fio { FirstName = "a" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));

            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendOrderedReportProperty(report, "NameEng.FirstName", "NameEng", 1, 1, 0);
            AppendOrderedReportProperty(report, "NameNative.FirstName", "NameNative", 2, 1, 1);
            AppendOrderedReportProperty(report, "Pin", "Pin", 3, 1, 2, "hyperlink(\"https://#host.luxoft.com/Sys/Web/#/employee/index?employee=#value\", \"#value\")");
            AppendOrderedReportProperty(report, "NameNative.LastName", "LastName", 4, 1, 3);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            var sheet = reportStream.GetSheet();

            var row = 2;
            var nameEng = sheet.Cells[row, 1].Value;
            var nameNative = sheet.Cells[row, 2].Value;
            var pin = sheet.Cells[row, 3].Value;

            nameEng.GetType().Should().Be(typeof(string));
            nameNative.GetType().Should().Be(typeof(string));
            pin.GetType().Should().Be(typeof(double)); // All numbers converted to double by Excel
            pin.Should().Be(10);
            sheet.Cells[row, 3].Hyperlink = new Uri($"https://{System.Environment.MachineName}.luxoft.com/Sys/Web/#/employee/index?employee={10}");
        }

        /// <summary>
        /// IADFRAME-1603 Сделать замену шаблона #value для всех формул в отчетном модуле
        /// </summary>
        [TestMethod]
        public void GetEmployeeReport_ReportWithFormulaReplace_CorrectTypeAndValue()
        {
            var hireDate = new DateTime(2010, 1, 2);
            this.DataHelper.SaveEmployee(hireDate: hireDate, pin: 10, nameEng: new Fio { FirstName = "abc" }, nameNative: new Fio { FirstName = "a" }, login: new string(Guid.NewGuid().ToString().Take(20).ToArray()));

            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var report = this.CreateReport<Employee>("EmployeePersistentProperties");
            AppendOrderedReportProperty(report, "NameEng.FirstName", "NameEng", 1, 1, 0);
            AppendOrderedReportProperty(report, "NameNative.FirstName", "NameNative", 2, 1, 1);
            AppendOrderedReportProperty(report, "HireDate", "HireDate", 3, 1, 2, "=CONCATENATE(MONTH(VALUE(#value)), \"-\", YEAR(VALUE(#value)))");
            AppendOrderedReportProperty(report, "NameNative.LastName", "LastName", 4, 1, 3);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>());

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            var sheet = reportStream.GetSheet();

            var row = 2;
            var nameEng = sheet.Cells[row, 1].Value;
            var nameNative = sheet.Cells[row, 2].Value;
            var hireDateCell = sheet.Cells[row, 3];

            nameEng.GetType().Should().Be(typeof(string));
            nameNative.GetType().Should().Be(typeof(string));
            hireDateCell.Value.Should().BeNull();
            hireDateCell.Formula.Should().Be($"=CONCATENATE(MONTH(VALUE({new DateTime(2010, 1, 2)})), \"-\", YEAR(VALUE({new DateTime(2010, 1, 2)})))");
        }

        [TestMethod]
        public void GetEmployeeReport_FilterByNullableDate_ShouldBeCorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var hireDate = DateTime.Now;
            var employeeFirstName = "Employee";
            var employeePin = (int)TimeSpan.FromTicks(DateTime.Now.Ticks).TotalDays;

            this.DataHelper.SaveEmployee(hireDate: hireDate, nameEng: new Fio { FirstName = employeeFirstName }, pin: employeePin);

            var versionPropertyDate = "Version";
            var pinPropertyName = "Pin";
            var nameEngPropertyName = "NameEng";
            var report = this.CreateReport<Employee>("EmployeeReport");

            AppendReportProperty(report, $"{nameEngPropertyName}.FirstName", nameEngPropertyName);
            AppendReportProperty(report, pinPropertyName, pinPropertyName);
            AppendReportProperty(report, versionPropertyDate, "Hire Date");

            AppendReportParameter<DateTime>(report, versionPropertyDate);
            AppendReportFilter(report, versionPropertyDate, ReportFilter.IsAfterOrNullOperator);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>() { { versionPropertyDate, 1 } });

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria(nameEngPropertyName, employeeFirstName)
                        .AddSearchCriteria(pinPropertyName, employeePin.ToString())
                        .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReportWithSecurityFields_FilterByNullableDate_ShouldBeCorrectReportExcel()
        {
            // Arrange
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var hireDate = DateTime.Now;
            var employeeFirstName = "Employee";
            var employeePin = (int)TimeSpan.FromTicks(DateTime.Now.Ticks).TotalDays;

            this.DataHelper.SaveEmployee(hireDate: hireDate, nameEng: new Fio { FirstName = employeeFirstName }, pin: employeePin);

            var businessUnitPropertyName = "CompanyLegalEntity";
            var versionPropertyDate = "Version";
            var pinPropertyName = "Pin";
            var nameEngPropertyName = "NameEng";
            var report = this.CreateReport<Employee>("EmployeeReport");

            AppendReportProperty(report, $"{nameEngPropertyName}.FirstName", nameEngPropertyName);
            AppendReportProperty(report, $"{businessUnitPropertyName}.Name", businessUnitPropertyName);
            AppendReportProperty(report, pinPropertyName, pinPropertyName);
            AppendReportProperty(report, versionPropertyDate, "Hire Date");

            AppendReportParameter<DateTime>(report, versionPropertyDate);
            AppendReportFilter(report, versionPropertyDate, ReportFilter.IsAfterOrNullOperator);

            var reportDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportDTO, new Dictionary<string, object>() { { versionPropertyDate, 1 } });

            // Act
            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            // Assert
            reportStream.GetSheet()
                        .CreateValidator()
                        .SetHeaderRowNumber(1)
                        .AddSearchCriteria(nameEngPropertyName, employeeFirstName)
                        .AddSearchCriteria(pinPropertyName, employeePin.ToString())
                        .ValidateExists();
        }

        [TestMethod]
        public void GetEmployeeReport_ChangeOrderProperties_CorrectBuild()
        {
            var sampleSystemGenericReportController = this.GetControllerEvaluator<SampleSystemGenericReportController>();
            var location = this.DataHelper.SaveLocation(name: "loc1");

            this.DataHelper.SaveLocation(name: "loc2");

            this.DataHelper.SaveEmployee(location: location, hrDepartment: this.DataHelper.SaveHRDepartment(codeNative: "CodeNative", code: "Code"));

            var report = this.CreateReport<Employee>("EmployeeReport");

            AppendReportProperty(report, "HRDepartment.Code", "hrDepartmentVisualIdentity");

            AppendReportProperty(report, "NameEng.FirstName", "name");

            AppendReportFilter(report, "Location", "eq", Guid.NewGuid().ToString(), false);

            var reportRichDTO = this.GetReport(this.SaveReport(report));

            var reportGenerationModel = CreateReportModel(reportRichDTO, new Dictionary<string, object>());

            var reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);

            // Act

            var properties = reportRichDTO.Properties;

            properties.Foreach(z => z.Order = (z.Order + 1) % properties.Count);

            sampleSystemGenericReportController.Evaluate(c => c.SaveReport(reportRichDTO.ToStrict()));

            reportRichDTO = sampleSystemGenericReportController.Evaluate(c => c.GetRichReport(reportRichDTO.Identity));

            reportGenerationModel = CreateReportModel(reportRichDTO, new Dictionary<string, object>());

            // Assert
            reportStream = sampleSystemGenericReportController.Evaluate(c => c.GetStream(reportGenerationModel));

            Assert.IsNotNull(reportStream);
        }


        private static void AppendReportParameter<T>(Report report, string name)
        {
            new ReportParameter(report)
            {
                Name = name,
                TypeName = typeof(T).FullName
            };
        }

        private static void AppendReportFilter(
            Report report,
            string propertyName,
            string operation)
        {
            new ReportFilter(report)
            {
                Property = propertyName,
                Value = propertyName,
                FilterOperator = operation,
                IsValueFromParameters = true
            };
        }

        private static void AppendReportFilter(
                Report report,
                string propertyName,
                string operation,
                string value,
                bool isValueFromParameters = true)
        {
            new ReportFilter(report)
            {
                Property = propertyName,
                Value = value,
                FilterOperator = operation,
                IsValueFromParameters = isValueFromParameters
            };
        }

        private static void AppendReportProperty(Report report, string name, string alias, int order = 1)
        {
            new ReportProperty(report)
            {
                Alias = alias,
                PropertyPath = name,
                Order = order
            };
        }

        private static void AppendOrderedReportProperty(Report report, string name, string alias, int sortOrder, int sortType, int order, string formula = null)
        {
            new ReportProperty(report)
            {
                Alias = alias,
                PropertyPath = name,
                SortOrdered = sortOrder,
                SortType = sortType,
                Order = order,
                Formula = formula
            };
        }


        private static ReportGenerationModelStrictDTO CreateReportModel(
                ReportRichDTO report,
                Dictionary<string, object> parameters = null)
        {
            var generationModel = new ReportGenerationModelStrictDTO();
            generationModel.Report = report.Identity;
            generationModel.Items = CreateReportGenerationValues(report, parameters ?? new Dictionary<string, object>());

            return generationModel;
        }

        private static List<ReportGenerationValueStrictDTO> CreateReportGenerationValues(
                ReportRichDTO report,
                Dictionary<string, object> parameters)
        {
            var values = parameters
                    .Select(kvp => CreateReportGenerationValue(report, kvp.Key, kvp.Value))
                    .Where(v => v != null)
                    .ToList();

            return values;
        }

        private static ReportGenerationValueStrictDTO CreateReportGenerationValue(
                ReportRichDTO report,
                string parameterName,
                object parameterValue)
        {
            var parameter = report.Parameters.SingleOrDefault(
                p => string.Equals(p.Name, parameterName, StringComparison.OrdinalIgnoreCase));

            if (parameter == null)
            {
                return null;
            }

            // ReSharper disable once MergeCastWithTypeCheck
            string value;

            if (parameterValue is Period)
            {
                var p = (Period)parameterValue;
                value = $"{p.StartDate}@{p.EndDate}";
            }
            else
            {
                value = parameterValue.ToString();
            }

            var result = new ReportGenerationValueStrictDTO
            {
                Value = value,
                Parameter = parameter.Identity
            };

            return result;
        }

        private ReportRichDTO GetReport(ReportIdentityDTO reportIdentityDTO)
        {
            return this.GetConfigurationControllerEvaluator().Evaluate(c => c.GetRichReport(reportIdentityDTO));
        }

        private ReportIdentityDTO SaveReport(Report report)
        {
            return this.Environment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context =>
            {
                var bll = context.Configuration.Logics.Report;
                bll.Save(report);
                return report.ToIdentityDTO();
            });
        }

        private Report CreateReport<T>(string name)
        {
            var report = new Report
            {
                Name = name,
                DomainTypeName = typeof(T).Name
            };

            return report;
        }
    }
}
