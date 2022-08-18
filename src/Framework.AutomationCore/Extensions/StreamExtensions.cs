using System;
using System.IO;

using Automation.Utils;

using ClosedXML.Excel;

namespace Automation.Extensions;

public static class StreamExtensions
{
    public static IXLWorksheet GetSheet(this Stream stream, string sheetName = "Data")
    {
        var excel = new XLWorkbook(stream);
        if (excel.Worksheets.TryGetWorksheet(sheetName, out var sheet))
        {
            return sheet;
        }

        throw new Exception($"Worksheet with name '{sheetName}' not found. Check worksheet name.");
    }

    public static void Save(this Stream stream, string folder = null, string fileName = null, string extension = "xlsx")
    {
        folder ??= ConfigUtil.TempFolder;
        fileName = fileName != null ? $"{fileName}.{extension}" : $"report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{extension}";
        var filePath = Path.Combine(folder, fileName);

        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        stream.CopyTo(fileStream);
        stream.Close();
        fileStream.Close();
    }
}