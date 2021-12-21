using System.Collections.Generic;
using System.Drawing;

using Framework.Report.Excel.EPPlus;
using Framework.Report.Parser;
using Framework.Report.Template;

namespace Framework.Report.Excel.Engine
{
    /// <summary>
    /// Copy of Common.Reporting.Excel.ExcelTemplateParser adjusted to work with ExcelEPPlusFileWrapper
    /// </summary>
    public class ExcelTemplateParser : TemplateParser
    {
        private IExcelFileWrapper excel;

        public ExcelTemplateParser(IExcelFileWrapper excel)
        {
            this.excel = excel;
        }

        public override IList<ReportTemplate> Parse()
        {
            IList<ReportTemplate> result = new List<ReportTemplate>();
            foreach (string worksheetName in this.excel.GetSheetsNamesExcludePivot())
            {
                this.excel.ActiveWorkSheetName = worksheetName;
                result.Add(this.Parse(worksheetName));
            }

            return result;
        }

        private ExcelReportTemplate Parse(string worksheetName)
        {
            ExcelReportTemplate result = new ExcelReportTemplate(worksheetName, ShiftDirection.NewPage);
            TemplateObjectParser parsingChain =
                new TemplateDetailsSectionParser(
                    new DetailTemplateFieldParser(
                        new TemplateFieldParser(
                            new StaticTemplateFieldParser(result))));

            object[,] excelValues = this.excel.GetWorkingRangeValues();
            for (int row = 1; row <= excelValues.GetLength(0); row++)
            {
                for (int col = 1; col <= excelValues.GetLength(1); col++)
                {
                    parsingChain.Parse(excelValues[row - 1, col - 1], new Point(col - 1, row - 1));
                }
            }

            return result;
        }
    }
}
