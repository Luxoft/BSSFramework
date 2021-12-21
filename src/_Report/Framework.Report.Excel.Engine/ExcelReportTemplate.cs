using Framework.Report.Template;

namespace Framework.Report.Excel.Engine
{
    /// <summary>
    /// Copy of Common.Reporting.Excel.ExcelReportTemplate adjusted to work with ExcelEPPlusFileWrapper
    /// </summary>
    public class ExcelReportTemplate : ReportTemplate
    {
        private string worksheetName;

        public ExcelReportTemplate(string worksheetName, ShiftDirection shiftDirection)
            : base(shiftDirection)
        {
            this.worksheetName = worksheetName;
        }

        public string WorksheetName
        {
            get { return this.worksheetName; }
        }
    }
}
