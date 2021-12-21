using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    public class EmptyCellValue : ICellValue
    {
        private EmptyCellValue()
        {
        }

        public static ICellValue Value { get; } = new EmptyCellValue();

        public void Apply(ExcelRange range)
        {
        }
    }
}