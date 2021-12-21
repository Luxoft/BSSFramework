using System.Collections;

using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    /// Данные для ячейки (группы ячеек) Excel-отчета с форматированием
    /// </summary>
    public interface ICellValue
    {
        void Apply(ExcelRange range);
    }
}
