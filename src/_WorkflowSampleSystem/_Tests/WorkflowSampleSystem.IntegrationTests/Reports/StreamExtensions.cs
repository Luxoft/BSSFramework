using System;
using System.IO;

using Automation.Utils;

using OfficeOpenXml;

namespace WorkflowSampleSystem.IntegrationTests.Reports
{
    public static class StreamExtensions
    {
        public static ExcelWorksheet GetSheet(this Microsoft.AspNetCore.Mvc.FileStreamResult actionResult, string sheetName = "Data")
        {
            return (actionResult).FileStream.GetSheet(sheetName);
        }

        public static void Save(this Microsoft.AspNetCore.Mvc.FileStreamResult actionResult, string folder = null, string fileName = null, string extension = ".xlsm")
        {
            (actionResult).Save(folder, fileName, extension);
        }

        public static ExcelWorksheet GetSheet(this Stream stream, string sheetName = "Data")
        {
            var mypackage = new ExcelPackage(stream);
            var sheet = mypackage.Workbook.Worksheets[sheetName];
            return sheet;
        }

        public static void Save(this Stream stream, string folder = null, string fileName = null, string extension = ".xlsm")
        {
            folder = folder ?? ConfigUtil.TempFolder;
            fileName = fileName ?? $"report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{extension}";
            var filePath = Path.Combine(folder, fileName);

            var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            stream.Close();
            fileStream.Close();
        }
    }
}
