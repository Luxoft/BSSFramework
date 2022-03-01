using System;
using System.Collections.Generic;
using System.Linq;

using Framework.CustomReports.Domain;
using Framework.CustomReports.Services.ExcelBuilder;
using Framework.CustomReports.Services.ExcelBuilder.CellValues;

using SampleSystem.CustomReports.Employee;

namespace SampleSystem.CustomReports.BLL
{
    public partial class EmployeeReportBLL
    {
        private readonly ExcelReportStreamService streamService = new ExcelReportStreamService();

        protected override IReportStream GetStream(EmployeeReport report, EmployeeReportParameter parameter)
        {
            var headers = new List<HeaderDesign<Domain.Employee>>
                          {
                              CreateHeader("Name", e => e.NameEng),
                              CreateHeader("Position", e => e.Position),
                              CreateHeader("Account name", e => e.AccountName),
                              CreateHeader("Cell phone", e => e.PersonalCellPhone),
                              CreateHeader("BirthDate", e => e.BirthDate),
                              CreateHeader("CreateDate", e => e.CreateDate),
                              CreateLognTextHeader("Long Test", e => @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, 
    sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam,
    quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat"),
                          };

            var parameterInfoItems = new List<EvaluateParameterInfoItem>
                                     {
                                         CreateParameter("ReportID", parameter.ReportId.ToString()),
                                     };

            var parameterInfo = new EvaluateParameterInfo(parameterInfoItems);

            var bll = this.Context.Logics.EmployeeFactory.Create(SampleSystemSecurityOperationCode.EmployeeView);
            var employees = bll.GetSecureQueryable().ToList();

            var stream = this.streamService.Generate(
                report.Name,
                headers,
                employees,
                parameterInfo);

            return new ReportStream(report.Name, ReportStreamType.Excel, stream);
        }

        private static HeaderDesign<Domain.Employee> CreateLognTextHeader(
            string headerName,
            Func<Domain.Employee, string> getCellValue)
        {
            var datePropertyInfo = new EvaluatePropertyInfo<Domain.Employee>(
                m =>
                {
                    var cellValue = getCellValue(m);
                    return new CellLongTextValue(cellValue);
                },
                headerName);

            var dateHeader = new HeaderDesign<Domain.Employee>(headerName, datePropertyInfo, 50);

            return dateHeader;
        }

        private static HeaderDesign<Domain.Employee> CreateHeader(
            string headerName,
            Func<Domain.Employee, DateTime?> getCellValue)
        {
            var datePropertyInfo = new EvaluatePropertyInfo<Domain.Employee>(
                m =>
                {
                    var cellValue = getCellValue(m);
                    return new CellDateNullableValue(cellValue);
                },
                headerName);

            var dateHeader = new HeaderDesign<Domain.Employee>(headerName, datePropertyInfo)
                             {
                                 AutoFit = true
                             };

            return dateHeader;
        }

        private static HeaderDesign<Domain.Employee> CreateHeader<T>(
                string headerName,
                Func<Domain.Employee, T> getCellValue)
        {
            var format = typeof(T) == typeof(float) ? "0.00" : null;



            var propertyInfo = new EvaluatePropertyInfo<Domain.Employee>(m => new CellValue<T>(getCellValue(m), format), headerName);

            var header = new HeaderDesign<Domain.Employee>(headerName, propertyInfo)
                         {
                             AutoFit = true
                         };

            return header;
        }

        private static EvaluateParameterInfoItem CreateParameter(string parameterName, string parameterValue)
        {
            return new EvaluateParameterInfoItem(parameterName, parameterValue);
        }
    }
}
