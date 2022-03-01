using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OfficeOpenXml;

namespace WorkflowSampleSystem.IntegrationTests.Reports
{
    public static class ExcelSheetExtensions
    {
        public static int RowCount(this ExcelWorksheet sheet, int headerRowCount = 1)
        {
            if (sheet.Dimension == null)
            {
                return 0;
            }

            return sheet.Dimension.End.Row - sheet.Dimension.Start.Row - (headerRowCount - 1);
        }

        public static int ColCount(this ExcelWorksheet sheet)
        {
            if (sheet.Dimension == null)
            {
                return 0;
            }

            return sheet.Dimension.End.Column - sheet.Dimension.Start.Column;
        }

        public static List<int> FindRows(this ExcelWorksheet sheet, Dictionary<int, string> criteria)
        {
            var rowList = new List<int>();
            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;

            for (int rowIndex = start.Row; rowIndex <= end.Row; rowIndex++)
            {
                bool rowFound = false;

                foreach (KeyValuePair<int, string> find in criteria)
                {
                    if (string.IsNullOrEmpty(find.Value) &&
                        string.IsNullOrEmpty(sheet.Cells[rowIndex, find.Key].GetValue<string>()))
                    {
                        rowFound = true;
                        break;
                    }

                    if (string.IsNullOrEmpty(sheet.Cells[rowIndex, find.Key].GetValue<string>())
                        || !sheet.Cells[rowIndex, find.Key].GetValue<string>().Equals(find.Value, StringComparison.InvariantCultureIgnoreCase))
                    {
                        rowFound = false;
                        break;
                    }

                    rowFound = true;
                }

                if (rowFound)
                {
                    rowList.Add(rowIndex);
                }
            }

            return rowList;
        }

        public static List<int> FindRows(this ExcelWorksheet sheet, Dictionary<string, string> criteria)
        {
            return sheet.FindRows(sheet.ConvertCriteria(criteria));
        }

        public static int FindRow(this ExcelWorksheet sheet, Dictionary<int, string> criteria)
        {
            var list = sheet.FindRows(criteria);

            return !list.Any() ? 0 : list[0];
        }

        public static int FindRow(this ExcelWorksheet sheet, Dictionary<string, string> criteria)
        {
            return sheet.FindRow(sheet.ConvertCriteria(criteria));
        }

        public static List<string> GetHeaders(this ExcelWorksheet sheet, int headerRowNumber = 1)
        {
            List<string> headers = new List<string>();

            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;

            for (var colIndex = start.Column; colIndex <= end.Column; colIndex++)
            {
                headers.Add(sheet.Cells[headerRowNumber, colIndex].GetValue<string>());
            }

            return headers;
        }

        public static void VerifyRowExists(
            this ExcelWorksheet sheet,
            Dictionary<string, string> criteria,
            int headerRowNumber = 1,
            bool shouldExists = true)
        {
            sheet.VerifyRowExists(sheet.ConvertCriteria(criteria), headerRowNumber, shouldExists);
        }

        public static void VerifyRowExists(
            this ExcelWorksheet sheet,
            Dictionary<int, string> criteria,
            int headerRowNmber = 1,
            bool shouldExist = true)
        {
            var headers = sheet.GetHeaders(headerRowNmber);

            if (shouldExist)
            {
                Assert.IsTrue(
                    sheet.FindRow(criteria) > 0,
                    "No row exists with values: {1}{0}",
                    criteria.Select(s => headers[s.Key - 1] + " = " + s.Value).Join(Environment.NewLine),
                    Environment.NewLine);
            }
            else
            {
                Assert.IsTrue(
                    sheet.FindRow(criteria) == 0,
                    "The following row mistakenly exists: {1}{0}",
                    criteria.Select(s => headers[s.Key - 1] + " = " + s.Value).Join(Environment.NewLine),
                    Environment.NewLine);
            }
        }

        public static ReportValidator CreateValidator(this ExcelWorksheet sheet)
        {
            return new ReportValidator(sheet);
        }

        private static Dictionary<int, string> ConvertCriteria(
            this ExcelWorksheet sheet,
            Dictionary<string, string> criteria)
        {
            var convertedCriteria = new Dictionary<int, string>();
            var start = sheet.Dimension.Start;
            var end = sheet.Dimension.End;

            foreach (KeyValuePair<string, string> find in criteria)
            {
                bool findColumn = false;

                for (int colIndex = start.Column; colIndex <= end.Column; colIndex++)
                {
                    if (sheet.Cells[1, colIndex].GetValue<string>().Equals(find.Key))
                    {
                        convertedCriteria.Add(colIndex, find.Value);
                        findColumn = true;
                        break;
                    }
                }

                if (!findColumn)
                {
                    throw new Exception(string.Format("Column '{0}' not found in the report", find.Key));
                }
            }

            if (criteria.Count != convertedCriteria.Count)
            {
                throw new Exception("Invalid criteria convertion");
            }

            return convertedCriteria;
        }
    }
}
