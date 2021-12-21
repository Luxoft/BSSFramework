using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    /// Данные для Excel-отчета, типа данных <see cref="TValue"/>, без дополнительного форматирования по умолчанию
    /// </summary>
    public class CellValue<TValue> : ICellValue
    {
        public CellValue(TValue value, string format = null)
        {
            this.Value = value;
            this.Format = format;
        }

        public TValue Value { get; }

        public string Format { get; }

        /// <summary>
        /// Заполняет значение ячейки и применяет форматирование
        /// </summary>
        /// <remarks>Вызывает <see cref="ApplyValue"/>, после чего <see cref="ApplyFormat"/></remarks>
        /// <param name="range">Группа ячеек для заполнения</param>
        public virtual void Apply(ExcelRange range)
        {
            this.ApplyValue(range);

            this.ApplyFormat(range);
        }

        protected virtual void ApplyValue(ExcelRange range)
        {
            range.Value = this.Value;
        }

        protected virtual void ApplyFormat(ExcelRange range)
        {
            if (!string.IsNullOrWhiteSpace(this.Format))
            {
                range.Style.Numberformat.Format = this.Format;
            }
        }
    }
}
