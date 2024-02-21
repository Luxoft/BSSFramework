using System.Reflection;

using ClosedXML.Excel;

namespace Automation.Extensions.Excel;

public static class ExcelSheetExtensions
{
    [Obsolete("Please use ConvertTo<> instead.")]
    public static List<T> ConvertToList<T>(this IXLWorksheet worksheet, int headerRow = 2)
        where T : new()
    {
        var mapping = typeof(T)
                      .GetProperties()
                      .Select(x => GetPropertyToColumnMapping(x, worksheet, headerRow));

        var collection = worksheet
            .RowsUsed()
            .Skip(headerRow)
            .Select(row => Map<T>(worksheet, row.RowNumber(), mapping));

        return collection.ToList();
    }

    public static IEnumerable<T> ConvertTo<T>(this IXLWorksheet worksheet, int headerRow = 2, string dataRangeName = "Items")
        where T : new()
    {
        var dataRange = worksheet.NamedRanges.FirstOrDefault(x => x.Name.Contains(dataRangeName));
        if (dataRange is null)
        {
            return [];
        }

        var mapping = typeof(T)
                         .GetProperties()
                         .Select(x => GetPropertyToColumnMapping(x, worksheet, headerRow));

        return dataRange.Ranges
                        .Cells()
                        .Select(x => x.WorksheetRow().RowNumber())
                        .Distinct()
                        .Select(row => Map<T>(worksheet, row, mapping));
    }

    private static T Map<T>(IXLWorksheet worksheet, int row, IEnumerable<(PropertyInfo propertyInfo, int columnIndex)> mapping)
        where T : new()
    {
        var instance = new T();
        foreach (var property in mapping)
        {
            if (property.propertyInfo.PropertyType == typeof(IEnumerable<string>))
            {
                property.propertyInfo.SetValue(
                    instance,
                    worksheet.Cell(row, property.columnIndex).GetValue<string>().Split(',').Select(i => i.Trim()));
            }
            else
            {
                property.propertyInfo.SetValue(
                    instance,
                    worksheet.Cell(row, property.columnIndex).GetValue<string>());
            }
        }

        return instance;
    }

    private static (PropertyInfo propertyInfo, int columnIndex) GetPropertyToColumnMapping(PropertyInfo propertyInfo, IXLWorksheet worksheet, int headerRow)
    {
        var columnAttribute = propertyInfo.GetCustomAttributes<Column>().FirstOrDefault();

        return (propertyInfo, columnAttribute?.Index ?? GetColumnByName(worksheet, columnAttribute?.Name ?? propertyInfo.Name, headerRow));
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
