using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Framework.Core;

namespace Framework.CustomReports.Services.ExcelBuilder.CellValues
{
    public static class CellValueFuncFactory
    {
        private const string ValueTemplate = "#value";

        private const string HostTemplate = "#host";

        private static readonly Regex HyperLinkPattern = new Regex("hyperlink\\(\"(.+)\",\\s+\"(.+)\"\\)", RegexOptions.IgnoreCase);

        public static Func<TSource, ICellValue> GetCellValueFunc<TSource, TProperty>(Func<TSource, TProperty> valueFunc, string format = null)
        {
            var result = new Func<object, string, ICellValue>(GetCellValue).CreateGenericMethod(typeof(TProperty));

            return z => (ICellValue)result.Invoke(typeof(CellValueFuncFactory), new object[] { valueFunc(z), format });
        }

        public static ICellValue GetRuntimeDefaultCell(object value, string formula)
        {
            var tryExtractValue = RuntimeExtractValueService.TryExtract(value);

            if (tryExtractValue == null)
            {
                return EmptyCellValue.Value;
            }

            var nextFormula = formula.NullIfEmpty();

            var result = (ICellValue)new Func<object, string, ICellValue>(GetCellValue)
                .CreateGenericMethod(value.GetType())
                .Invoke(typeof(CellValueFuncFactory), new object[] { tryExtractValue, nextFormula });

            return result;
        }

        private static ICellValue GetCellValue<TProperty>(TProperty value, string formula = null)
        {
            var cellValues = new List<ICellValue>() { BorderCellValue.Value };

            if (formula != null)
            {
                // hyperlink("https://#host.luxoft.com/sys/web#/employee/index?employee=#value", "#value")
                var match = HyperLinkPattern.Match(formula);
                if (match.Success)
                {
                    var adaptiveUri = Regex.Replace(match.Groups[1].Value, HostTemplate, Environment.MachineName, RegexOptions.IgnoreCase);

                    adaptiveUri = Regex.Replace(adaptiveUri, ValueTemplate, value.ToString(), RegexOptions.IgnoreCase);

                    cellValues.Add(new CellUrlValue<TProperty>(new Uri(adaptiveUri), value));
                }
                else
                {
                    var replaced = formula.Replace(ValueTemplate, value.ToString());
                    cellValues.Add(new CellFormulaValue(replaced));
                }
            }
            else if (typeof(TProperty) == typeof(DateTime))
            {
                cellValues.Add(new CellDateValue((DateTime)Convert.ChangeType(value, typeof(DateTime))));
            }
            else if (typeof(TProperty) == typeof(decimal))
            {
                AddNumberValue<TProperty, decimal>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(decimal?))
            {
                AddNumberValue<TProperty, decimal?>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(float))
            {
                AddNumberValue<TProperty, float>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(float?))
            {
                AddNumberValue<TProperty, float?>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(double))
            {
                AddNumberValue<TProperty, double>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(double?))
            {
                AddNumberValue<TProperty, double?>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(int))
            {
                AddNumberValue<TProperty, int>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(int?))
            {
                AddNumberValue<TProperty, int?>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(long))
            {
                AddNumberValue<TProperty, long>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(long?))
            {
                AddNumberValue<TProperty, long?>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(short))
            {
                AddNumberValue<TProperty, short>(value, cellValues);
            }
            else if (typeof(TProperty) == typeof(short?))
            {
                AddNumberValue<TProperty, short?>(value, cellValues);
            }
            else 
            {
                cellValues.Add(new CellValue<string>(value.ToString()));
            }

            if (cellValues.Count > 1)
            {
                return cellValues.Combine();
            }

            return cellValues[0];
        }

        private static void AddNumberValue<TProperty, TCell>(TProperty value, ICollection<ICellValue> cellValues) => cellValues.Add(new CellNumberValue<TCell>((TCell)Convert.ChangeType(value, typeof(TCell))));
    }
}
