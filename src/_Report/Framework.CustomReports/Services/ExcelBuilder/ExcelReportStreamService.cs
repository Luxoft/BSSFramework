using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using Framework.Core;
using Framework.CustomReports.Services.ExcelBuilder.CellValues;

using JetBrains.Annotations;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    [Obsolete("Use PlaneExcelReportStreamService. ExcelReportStreamService with very slow perfomance")]
    public class ExcelReportStreamService
    {
        public Stream Generate<TAnon>(
            string reportName,
            IList<HeaderDesign<TAnon>> headers,
            IList<TAnon> anonValues,
            EvaluateParameterInfo parameterModel)
        {
            var anonValuesGrouped = new List<IList<TAnon>> { anonValues };
            return this.GenerateGrouped(
                reportName,
                headers,
                anonValuesGrouped,
                parameterModel,
                false,
                new GroupedValueDesign<IList<TAnon>>[0],
                new GroupedValueDesign<List<IList<TAnon>>>[0]);
        }

        public Stream GenerateGrouped<TAnon>(
             string reportName,
             IList<HeaderDesign<TAnon>> headers,
             List<IList<TAnon>> anonValuesGrouped,
             EvaluateParameterInfo parameterModel,
             bool showGrouped,
             IEnumerable<GroupedValueDesign<IList<TAnon>>> subGroupedValues,
             IEnumerable<GroupedValueDesign<List<IList<TAnon>>>> totalGroupedValues)
        {
            var subGroupedFuncDict = subGroupedValues.ToDictionary(z => z.ColumnHeader, z => z);
            var totalFuncDict = totalGroupedValues.ToDictionary(z => z.ColumnHeader, z => z);

            var columnStart = 1;

            using (var myPackage = new ExcelPackage())
            {
                var dataSheet = myPackage.Workbook.Worksheets.Add("Data");
                var paramSheet = myPackage.Workbook.Worksheets.Add("Parameters");

                dataSheet.Protection.AllowInsertHyperlinks = true;

                this.GenerateReportParameters(reportName, parameterModel, paramSheet);

                var headerDesignInfo = new HeaderDesignInfo(headers.GetDeep(), headers.GetAllElements(z => z.SubHeaders).Count(z => !z.SubHeaders.Any()));

                foreach (var headerDesign in headers)
                {
                    columnStart = this.Render(headerDesign, headerDesignInfo, 1, columnStart, subGroupedFuncDict, totalFuncDict, dataSheet, showGrouped, anonValuesGrouped);
                }

                if (!showGrouped)
                {
                    var dimension = dataSheet.Dimension;
                    var range = dataSheet.Cells[dimension.Address];

                    var table = dataSheet.Tables.Add(range, "reportData");
                    table.ShowTotal = false;
                    table.TableStyle = TableStyles.Light9;

                    dataSheet.View.FreezePanes(2, 1);
                }

                var result = new MemoryStream();

                myPackage.SaveAs(result);

                result.Seek(0, SeekOrigin.Begin);

                return result;
            }
        }

        protected virtual int RenderGroupRow<TAnon>(
                [NotNull] IEnumerable<TAnon> anonValuesGroupedItem,
                [NotNull] ExcelWorksheet sheet,
                int currentRow,
                int columnIndex,
                Func<TAnon, ICellValue> formulaFunc)
        {
            if (anonValuesGroupedItem == null)
            {
                throw new ArgumentNullException(nameof(anonValuesGroupedItem));
            }

            if (sheet == null)
            {
                throw new ArgumentNullException(nameof(sheet));
            }

            foreach (var item in anonValuesGroupedItem)
            {
                var valueCell = sheet.Cells[currentRow, columnIndex];
                currentRow++;

                if (formulaFunc == null)
                {
                    continue;
                }

                var evaluatedFormula = formulaFunc(item);

                evaluatedFormula?.Apply(valueCell);
            }

            return currentRow;
        }

        protected virtual int Render<TAnon>(
            HeaderDesign<TAnon> headerDesign,
            HeaderDesignInfo headerDesignInfo,
            int rowStart,
            int columnStart,
            Dictionary<string, GroupedValueDesign<IList<TAnon>>> subGroupedFuncDict,
            Dictionary<string, GroupedValueDesign<List<IList<TAnon>>>> totalFuncDict,
            ExcelWorksheet sheet,
            bool showGrouped,
            List<IList<TAnon>> anonValuesGrouped)
        {
            if (headerDesign.SubHeaders.Any())
            {
                var excelRange = sheet.Cells[rowStart, columnStart, rowStart, columnStart + headerDesign.GetFinalCount() - 1];
                excelRange.Merge = true;
                this.SetHeaderCellStyle(excelRange);
                excelRange.Value = headerDesign.Caption;

                var lastColumnIndex = columnStart;
                foreach (var subHeader in headerDesign.SubHeaders)
                {
                    lastColumnIndex = this.Render(subHeader, new HeaderDesignInfo(headerDesignInfo.MaxDeep - 1, headerDesignInfo.Count), rowStart + 1, lastColumnIndex, subGroupedFuncDict, totalFuncDict, sheet, showGrouped, anonValuesGrouped);
                }

                return lastColumnIndex;
            }

            var calcRowStart = rowStart + headerDesignInfo.MaxDeep;
            var range = sheet.Cells[rowStart, columnStart, calcRowStart - 1, columnStart];

            if (headerDesign.Width.HasValue)
            {
                sheet.Column(columnStart).Width = headerDesign.Width.Value;
            }

            if (showGrouped)
            {
                range.Merge = true;
                this.SetHeaderCellStyle(range);
            }

            range.Value = headerDesign.Caption;

            var evaluatePropertyInfo = headerDesign.EvaluateProperty;

            if (evaluatePropertyInfo == null)
            {
                return columnStart + 1;
            }

            var formulaFunc = evaluatePropertyInfo.GetCellValueFunc;

            var currentRow = calcRowStart;

            foreach (var anonValuesGroupedItem in anonValuesGrouped)
            {
                currentRow = this.RenderGroupRow(anonValuesGroupedItem, sheet, currentRow, columnStart, formulaFunc);

                if (!showGrouped)
                {
                    continue;
                }

                currentRow++;

                GroupedValueDesign<IList<TAnon>> groupedFunc;

                if (!subGroupedFuncDict.TryGetValue(evaluatePropertyInfo.Alias, out groupedFunc))
                {
                    continue;
                }

                var cellStyle = sheet.Cells[currentRow - 1, 1, currentRow - 1, headerDesignInfo.Count].Style.Fill;

                cellStyle.PatternType = ExcelFillStyle.Solid;

                cellStyle.BackgroundColor.SetColor(Color.FromArgb(221, 235, 247));

                var cell = sheet.Cells[currentRow - 1, columnStart];

                groupedFunc.Action(anonValuesGroupedItem, cell);
            }

            if (showGrouped)
            {
                GroupedValueDesign<List<IList<TAnon>>> totalGroupedValueDesign;

                if (totalFuncDict.TryGetValue(evaluatePropertyInfo.Alias, out totalGroupedValueDesign))
                {
                    var cell = sheet.Cells[currentRow, columnStart];

                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Color.SetColor(Color.White);

                    totalGroupedValueDesign.Action(anonValuesGrouped, cell);
                }
            }

            evaluatePropertyInfo.DesignColumnAction(sheet.Column(columnStart));

            if (headerDesign.AutoFit)
            {
                sheet.Column(columnStart).AutoFit();
            }

            return columnStart + 1;
        }

        protected virtual void GenerateReportParameters(string reportName, EvaluateParameterInfo parameterInfo, ExcelWorksheet paramSheet)
        {
            var reportNameCells = paramSheet.Cells[1, 1];

            var blueColor = Color.FromArgb(54, 96, 146);

            reportNameCells.Style.Font.Size = 18;
            reportNameCells.Style.Font.Bold = true;
            reportNameCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            reportNameCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            reportNameCells.Style.Font.Color.SetColor(blueColor);
            reportNameCells.Value = reportName;

            if (!parameterInfo.Items.Any())
            {
                return;
            }

            paramSheet.Row(1).CustomHeight = true;
            paramSheet.Row(1).Height = 48;

            var startRow = 3;
            var startColumn = 1;

            var startParameterCell = paramSheet.Cells[startRow, startColumn];

            // i18n suggested here #IADFRAME-948 is impossible so far
            startParameterCell.Value = "Name";

            var startParameterCellValue = paramSheet.Cells[startRow, startColumn + 1];
            startParameterCellValue.Value = "Value";

            var values = parameterInfo.Items.Select(z => new { Name = z.Name, Value = z.Value }).ToList();

            // foreach made code less readable
            for (var q = 0; q < values.Count; q++)
            {
                var nameCell = paramSheet.Cells[startRow + q + 1, startColumn];
                var valueCell = paramSheet.Cells[startRow + q + 1, startColumn + 1];

                nameCell.Value = values[q].Name;
                valueCell.Value = values[q].Value;
            }

            var range = paramSheet.Cells[startRow, startColumn, startRow + values.Count, startColumn + 1];

            var table = paramSheet.Tables.Add(range, "reportParameters");
            table.ShowTotal = false;
            table.TableStyle = TableStyles.Light9;

            range.AutoFitColumns();
        }

        protected virtual void SetHeaderCellStyle(ExcelRange headerCell)
        {
            var cellStyle = headerCell.Style.Fill;
            cellStyle.PatternType = ExcelFillStyle.Solid;
            cellStyle.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213));

            headerCell.Style.Font.Bold = true;
            headerCell.Style.Font.Color.SetColor(Color.White);
            headerCell.Style.WrapText = true;
            headerCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            headerCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
    }
}
