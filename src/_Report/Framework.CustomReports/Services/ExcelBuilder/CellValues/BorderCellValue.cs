using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    public class BorderCellValue : ICellValue
    {
        private BorderCellValue()
        {
        }

        public static ICellValue Value { get; } = new BorderCellValue();

        public void Apply(ExcelRange range)
        {
            range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
    }
}