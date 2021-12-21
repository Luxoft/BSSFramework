using System;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    /// Данные для Excel-отчета, типа данных DateTime, форматирование по умолчанию "dd MMM yyyy"
    /// </summary>
    public class CellDateNullableValue : CellValue<DateTime?>
    {
        public CellDateNullableValue(DateTime? value, string format = null)
            : base(value, string.IsNullOrWhiteSpace(format) ? CellDateValue.DefaultFormat : format)
        {
        }
    }
}
