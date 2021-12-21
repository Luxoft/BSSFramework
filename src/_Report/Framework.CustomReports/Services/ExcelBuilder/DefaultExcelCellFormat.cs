using System;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public static class DefaultExcelCellFormat
    {
        private static string dateTimeCellFormat = "dd MMM yyyy";

        private static string stringCellFormat = "Text";

        public static string GetDefaultFormat(Type type)
        {
            if (type == typeof(DateTime))
            {
                return dateTimeCellFormat;
            }

            if (type == typeof(string))
            {
                return stringCellFormat;
            }

            return null;
        }
    }
}
