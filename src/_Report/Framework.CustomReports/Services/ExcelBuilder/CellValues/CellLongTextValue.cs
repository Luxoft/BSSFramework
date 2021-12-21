using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    /// Данные для Excel-отчета, длинный текст, для которого применяется WrapText
    /// </summary>
    public class CellLongTextValue : CellValue<string>
    {
        public CellLongTextValue(string value)
            : base(value, null)
        {
        }

        protected override void ApplyFormat(ExcelRange range)
        {
            range.Style.WrapText = true;
        }
    }
}
