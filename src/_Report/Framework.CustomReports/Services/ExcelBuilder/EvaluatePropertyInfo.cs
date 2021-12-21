using System;

using Framework.CustomReports.Services.ExcelBuilder.CellValues;

using OfficeOpenXml;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public static class EvaluatePropertyInfo
    {
        public static EvaluatePropertyInfo<T> Create<T, TProperty>(Func<T, TProperty> getPropertyValueFunc, string @alias, Action<ExcelColumn> designAction = null)
        {
            return new EvaluatePropertyInfo<T>(CellValueFuncFactory.GetCellValueFunc(getPropertyValueFunc), @alias, designAction);
        }

        public static EvaluatePropertyInfo<T> CreateAutoFill<T, TProperty>(Func<T, TProperty> getPropertyValueFunc, string @alias)
        {
            return new EvaluatePropertyInfo<T>(CellValueFuncFactory.GetCellValueFunc(getPropertyValueFunc), @alias, z => z.AutoFit());
        }
    }

    public class EvaluatePropertyInfo<TSource>
    {
        private static readonly Action<ExcelColumn> EmptyColumnAction = _ => { };

        public EvaluatePropertyInfo(Func<TSource, ICellValue> getCellValueFunc, string alias, Action<ExcelColumn> designColumnAction = null)
        {
            if (getCellValueFunc == null)
            {
                throw new ArgumentNullException(nameof(getCellValueFunc));
            }

            if (alias == null)
            {
                throw new ArgumentNullException(nameof(alias));
            }

            this.Alias = alias;
            this.GetCellValueFunc = getCellValueFunc;
            this.DesignColumnAction = designColumnAction ?? EmptyColumnAction;
        }

        public string Alias { get; }

        public Func<TSource, ICellValue> GetCellValueFunc { get; }

        public Action<ExcelColumn> DesignColumnAction { get; }

        public static EvaluatePropertyInfo<TSource> CreateWithDefaultDesign<TProperty>(Func<TSource, TProperty> func, string caption)
        {
            return new EvaluatePropertyInfo<TSource>(
                                                     CellValueFuncFactory.GetCellValueFunc(func),
                                                     caption,
                                                     z => z.AutoFit());
        }
    }
}
