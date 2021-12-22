using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ClosedXML.Excel;

namespace Automation.Extensions.Excel
{
    public static class ExcelSheetExtensions
    {
        public static List<T> ConvertToList<T>(this IXLWorksheet worksheet, int headerRow = 2)
            where T : new()
        {
            var properties = typeof(T)
                .GetProperties()
                .Select(p => new { PropertyInfo = p, Column = p.GetCustomAttributes<Column>().FirstOrDefault()?.Name ?? p.Name })
                .ToList();

            var collection = worksheet
                .RowsUsed()
                .Skip(headerRow)
                .Select(
                    row =>
                    {
                        var instance = new T();
                        properties.ForEach(
                            property =>
                            {
                                var colIndex = worksheet.GetColumnByName(property.Column, headerRow);

                                var cell = worksheet.Cell(row.RowNumber(), colIndex);

                                if (property.PropertyInfo.PropertyType == typeof(IEnumerable<string>))
                                {
                                    property.PropertyInfo.SetValue(instance, cell.GetValue<string>().Split(',').Select(i => i.Trim()));
                                }
                                else
                                {
                                    property.PropertyInfo.SetValue(instance, cell.GetValue<string>());
                                }
                            });

                        return instance;
                    });

            return collection.ToList();
        }

        private static int GetColumnByName(this IXLWorksheet ws, string name, int headerRow)
        {
            if (ws == null) throw new ArgumentNullException(nameof(ws));

            try
            {
                return ws.Row(headerRow).Cells().First(c => c.Value.ToString() == name).WorksheetColumn().ColumnNumber();
            }
            catch (Exception)
            {
                throw new Exception($"No specified column with name '{name}'");
            }
        }
    }
}
