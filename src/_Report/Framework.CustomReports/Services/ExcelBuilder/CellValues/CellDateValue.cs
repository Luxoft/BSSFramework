using System;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    /// Данные для Excel-отчета, типа данных DateTime, форматирование по умолчанию "dd MMM yyyy"
    /// </summary>
    public class CellDateValue : CellValue<DateTime>
    {
        public const string DefaultFormat = "dd MMM yyyy";

        public CellDateValue(DateTime value, string format = null)
            : base(value, string.IsNullOrWhiteSpace(format) ? DefaultFormat : format)
        {
        }
    }
}
