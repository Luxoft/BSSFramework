using System;
using System.Drawing;

using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    /// <summary>
    /// Размещает данные в ячейке как <seealso cref="CellValue&lt;TValue&gt;"/>, но добавляет <see cref="Url"/> в поле Hyperlink ячейки и применяет форматирование Url
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class CellUrlValue<TValue> : CellValue<TValue>
    {
        public CellUrlValue(Uri url, TValue value, string format = null)
            : base(value, format)
        {
            this.Url = url;
        }

        public Uri Url { get; }

        protected override void ApplyValue(ExcelRange range)
        {
            base.ApplyValue(range);

            if (this.Url != null)
            {
                range.Hyperlink = this.Url;
            }
        }

        protected override void ApplyFormat(ExcelRange range)
        {
            base.ApplyFormat(range);

            range.Style.Font.UnderLine = true;
            range.Style.Font.Color.SetColor(Color.Blue);
        }
    }
}
