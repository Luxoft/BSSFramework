using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public class PlainExcelReportStreamService : IExcelReportStreamService
    {
        public Stream Generate<TAnon>(
            string reportName,
            ExcelHeaderDesign<TAnon>[] headers,
            IList<TAnon> values,
            EvaluateParameterInfo parameterModel)
        {
            var columnStart = 1;

            using (var myPackage = new ExcelPackage())
            {
                var dataSheet = myPackage.Workbook.Worksheets.Add("Data");

                var paramSheet = myPackage.Workbook.Worksheets.Add("Parameters");
                this.GenerateReportParameters(reportName, parameterModel, paramSheet);

                dataSheet.Protection.AllowInsertHyperlinks = true;

                var rowStart = 1;

                var rentArray = new object[values.Count, 1];

                foreach (var headerDesign in headers)
                {
                    columnStart = this.Render(headerDesign, rowStart, columnStart, dataSheet, values, rentArray);
                }

                if (values.Count == 0)
                {
                    dataSheet.Cells[1, 1, dataSheet.Dimension.Rows, dataSheet.Dimension.Columns].AutoFitColumns();
                }
                else
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

        protected int Render<TAnon>(
            ExcelHeaderDesign<TAnon> headerDesign,
            int rowStart,
            int columnStart,
            ExcelWorksheet sheet,
            IList<TAnon> values,
            object[,] rentArray)
        {
            var calcRowStart = rowStart + 1;
            var range = sheet.Cells[rowStart, columnStart, calcRowStart - 1, columnStart];

            this.SetHeaderCellStyle(range);
            range.Value = headerDesign.Caption;

            var evaluatePropertyInfo = headerDesign.DesignProperty;

            if (evaluatePropertyInfo != null && values.Count > 0)
            {
                this.Render(values, sheet, calcRowStart, columnStart, evaluatePropertyInfo, rentArray);
            }

            return columnStart + 1;
        }

        private int Render<TAnon>(
                IList<TAnon> values,
                ExcelWorksheet sheet,
                int currentRow,
                int columnIndex,
                ExcelDesignProperty<TAnon> designPropertyInfo,
                object[,] rentArray)
        {
            var range = sheet.Cells[currentRow, columnIndex, values.Count + currentRow - 1, columnIndex];

            if (1 == values.Count)
            {
                range.Value = designPropertyInfo.GetRenderedValue(values[0]);
            }
            else
            {
                for (int q = 0; q < values.Count; q++)
                {
                    rentArray[q, 0] = designPropertyInfo.GetRenderedValue(values[q]);
                }

                range.Value = rentArray;
            }

            designPropertyInfo.ApplyDesignAction(sheet.Column(columnIndex), range);

            return currentRow;
        }

        protected virtual void SetHeaderCellStyle(ExcelRange headerCell)
        {
            var cellStyle = headerCell.Style.Fill;
            cellStyle.PatternType = ExcelFillStyle.Solid;
            cellStyle.BackgroundColor.SetColor(Color.FromArgb(91, 155, 213));

            headerCell.Style.Font.Bold = true;
            headerCell.Style.Font.Color.SetColor(Color.White);
            //headerCell.Style.WrapText = true;
            headerCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            headerCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            headerCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        }
    }
}
