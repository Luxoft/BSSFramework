using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    public static class CellValueExtension
    {
        public static ICellValue Combine(this IEnumerable<ICellValue> values)
        {
            return new CompositeCellValue(values.ToArray());
        }

        private class CompositeCellValue : ICellValue
        {
            private readonly ICellValue[] values;

            public CompositeCellValue(ICellValue[] values)
            {
                this.values = values;
            }

            public void Apply(ExcelRange range)
            {
                this.values.Foreach(z => z.Apply(range));
            }
        }
    }
}