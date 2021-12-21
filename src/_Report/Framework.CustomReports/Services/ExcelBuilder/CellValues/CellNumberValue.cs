namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    ///     Данные для Excel-отчета, типа данных <see cref="TValue" />, выставляет свойству NumberFormat.Format значение
    ///     General
    ///     />
    /// </summary>
    public class CellNumberValue<TValue> : CellValue<TValue>
    {
        private const string DefaultFormat = "General";

        public CellNumberValue(TValue value)
            : base(value, DefaultFormat)
        {
        }
    }
}
