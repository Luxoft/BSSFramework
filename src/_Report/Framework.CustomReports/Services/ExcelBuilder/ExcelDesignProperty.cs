using System;

using Framework.Core;

using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public class ExcelDesignProperty<TSource>
    {
        private static Action<ExcelColumn, ExcelRange> emptyColumnAction = (_, __) => { };

        public ExcelDesignProperty(
            string designName,
            Func<TSource, object> getRenderedValueFunc,
            string defaultCellFormat,
            ExcelDesignFormula formula = null,
            Action<ExcelColumn, ExcelRange> designColumnAction = null)
        {
            this.DesignName = designName;

            this.GetRenderedValue = getRenderedValueFunc;

            var applyCustomDesignAction = designColumnAction ?? emptyColumnAction;

            var applyFormula = formula;

            this.ApplyDesignAction = (column, range) =>
                                     {
                                         if (!string.IsNullOrWhiteSpace(defaultCellFormat))
                                         {
                                             range.Style.Numberformat.Format = defaultCellFormat;
                                         }

                                         applyCustomDesignAction(column, range);

                                         applyFormula?.ApplyFormula(range);
                                     };
        }

        public string DesignName { get; }

        public Func<TSource, object> GetRenderedValue { get; }

        public Action<ExcelColumn, ExcelRange> ApplyDesignAction { get; }
    }
}
