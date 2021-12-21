using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    /// Формула для ячеек Excel-отчета
    /// </summary>
    public class CellFormulaValue : CellValue<string>
    {
        public CellFormulaValue(string formula, string format = null)
            : base(formula, format)
        {
        }

        protected override void ApplyValue(ExcelRange range)
        {
            range.Formula = this.Value;
        }
    }
}
