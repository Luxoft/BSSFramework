using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Framework.Report.Excel.EPPlus
{
    /// <summary>
    /// It should really avoid creating temporary files, everything can and should stay in memory streams.
    /// </summary>
    public class ExcelEPPlusFileWrapper : IExcelFileWrapper
    {
        private readonly string fileName;

        private ExcelPackage package;

        private MemoryStream packageStream;

        public ExcelEPPlusFileWrapper(string fileName) => this.fileName = fileName;

        private ExcelWorksheet ActiveSheet
        {
            get
            {
                if (this.package?.Workbook == null)
                {
                    return null;
                }

                return !string.IsNullOrWhiteSpace(this.ActiveWorkSheetName)
                           ? this.package.Workbook.Worksheets[this.ActiveWorkSheetName]
                           : this.package.Workbook.Worksheets.FirstOrDefault();
            }
        }

        public string ActiveWorkSheetName { get; set; }

        public object[,] GetWorkingRangeValues()
        {
            if (null == this.ActiveSheet.Dimension)
            {
                return new object[0, 0];
            }

            return this.GetRangeValues(
                this.ActiveSheet.Dimension.Start.Row,
                this.ActiveSheet.Dimension.Start.Column,
                this.ActiveSheet.Dimension.Rows,
                this.ActiveSheet.Dimension.Columns);
        }

        public object[,] GetWorkingRangeFormulas() =>
            this.GetRangeFormulas(
                this.ActiveSheet.Dimension.Start.Row,
                this.ActiveSheet.Dimension.Start.Column,
                this.ActiveSheet.Dimension.Rows,
                this.ActiveSheet.Dimension.Columns);

        public object[,] GetRangeFormulas(int row, int column, int height, int width) =>
            this.GetRangeContents(row, column, height, width, cell => cell.FormulaR1C1);

        public void SetPassword(string password) => this.package.Workbook.Protection.SetPassword(password);

        public void ShiftRows(int row, int height, int times)
        {
            if (times == 0 || height <= 0)
            {
                return;
            }

            if (times == -1)
            {
                for (var i = 0; i < height; i++)
                {
                    this.ActiveSheet.DeleteRow(row + i);
                }
            }
            else
            {
                this.ActiveSheet.InsertRow(row + 1, height * times, row);
                for (var i = 0; i < times; i++)
                {
                    this.CopyRows(row, height, i);
                }
            }
        }

        public void Open(byte[] excelFileBytes)
        {
            this.Dispose();
            this.packageStream = new MemoryStream(excelFileBytes);
            this.package = new ExcelPackage(this.packageStream);
        }

        public void SaveAs(string name) => this.package.SaveAs(new FileInfo(name));

        /// <summary>
        /// Saving as pdf is not supported. Saved as excel instead.
        /// </summary>
        /// <param name="name"></param>
        public void SaveAsPdf(string name) => this.SaveAs(name);

        public void Save() => this.SaveAs(this.fileName);

        public void SaveToStream(Stream stream) => this.package.SaveAs(stream);

        /// <summary>
        /// Macro will run on first open of the document (not after file generation, as it was in previous implementation
        /// </summary>
        /// <param name="macroName"></param>
        public void RunMacro(string macroName) => this.RunMacroInternal(macroName);

        /// <summary>
        /// Never tested with parameters since no reports with parametrized macro found in ED
        /// </summary>
        /// <param name="macroName"></param>
        /// <param name="param1"></param>
        public void RunMacro(string macroName, object param1) => this.RunMacroInternal(macroName, param1);

        /// <summary>
        ///
        /// </summary>
        /// <param name="macroName"></param>
        /// <param name="disableScreenUpdating">Not used</param>
        public void RunMacro2(string macroName, bool disableScreenUpdating) => this.RunMacro(macroName);

        public void SetRange(int row, int column, string worksheetName, object[,] values)
        {
            this.ActiveWorkSheetName = worksheetName;
            this.SetRange(row, column, values);
        }

        public void SetRange(int row, int column, object[,] values)
        {
            var sheet = this.ActiveSheet;
            var rowsCount = values.GetLength(0);
            var columnsCount = values.GetLength(1);
            for (var r = 0; r < rowsCount; r++)
            {
                for (var c = 0; c < columnsCount; c++)
                {
                    var value = values[r, c];
                    switch (value)
                    {
                        case null:
                            sheet.Cells[r + row, c + column].Value = string.Empty;
                            continue;
                        case string stringValue when stringValue.StartsWith("="):
                            sheet.Cells[r + row, c + column].FormulaR1C1 = stringValue;
                            continue;
                        default:
                            sheet.Cells[r + row, c + column].Value = value;
                            break;
                    }
                }
            }
        }

        public IList<string> GetSheetsNamesExcludePivot() =>
            this.package.Workbook.Worksheets
                .Where(ws => ws.PivotTables.Count == 0)
                .Select(ws => ws.Name)
                .ToList();

        public void AutoFitColumns()
        {
            foreach (var excelWorksheet in this.package.Workbook.Worksheets.Where(z => null != z.Dimension))
            {
                excelWorksheet.Cells.AutoFitColumns();
            }
        }

        public void Dispose()
        {
            if (this.package != null)
            {
                this.package.Dispose();
                this.package = null;
            }

            if (this.packageStream != null)
            {
                this.packageStream.Dispose();
                this.packageStream = null;
            }
        }

        public object[,] GetRangeValues(int row, int column, int height, int width) =>
            this.GetRangeContents(row, column, height, width, cell => cell.Value);

        public void InsertRowUnder(ExcelField target) => this.InsertRowUnder(target.Row, 1);

        public void InsertRowUnder(int row, int height, string worksheetName)
        {
            this.ActiveWorkSheetName = worksheetName;
            this.InsertRowUnder(row, height);
        }

        public void InsertRowUnder(int row, int height)
        {
            this.ActiveSheet.InsertRow(row, height, row);
            this.CopyRows(row, height, 0);
        }

        public void DeleteRow(string workSheetName, int rowIndex)
        {
            this.ActiveWorkSheetName = workSheetName;
            this.DeleteRow(rowIndex);
        }

        public void DeleteRow(int rowIndex) => this.ActiveSheet.DeleteRow(rowIndex);

        public void InsertColToRight(ExcelField target)
        {
            var worksheet = this.package.Workbook.Worksheets[target.WorksheetName];
            if (null == worksheet)
            {
                throw new ArgumentException($"There is no worksheet with name '{target.WorksheetName}'.");
            }

            worksheet.InsertColumn(target.Column, 1, target.Column);
        }

        /// <summary>
        /// Saving as Html is not supported. Saved as excel instead.
        /// </summary>
        /// <param name="name"></param>
        public void SaveAsHtml(string name) => this.SaveAs(name);

        public void SaveAsXlsx() => this.Save();

        public void SetFields(int sheetNumber, IList<ExcelField> setFieldInfos) =>
            this.SetFields(this.package.Workbook.Worksheets[sheetNumber], setFieldInfos);

        public void SetFields(string sheetName, IList<ExcelField> setFieldInfos) =>
            this.SetFields(this.package.Workbook.Worksheets[sheetName], setFieldInfos);

        /// <summary>
        /// Never tested with parameters since no reports with parametrized macro found in ED
        /// </summary>
        /// <param name="macroName"></param>
        /// <param name="disableScreenUpdating">Not used</param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        public void RunMacro(string macroName, bool disableScreenUpdating, object param1 = null, object param2 = null) =>
            this.RunMacroInternal(macroName, param1, param2);

        public CellFormat[,] GetRangeFormatInfo(int row, int column, int height, int width)
        {
            var result = new CellFormat[width, height];
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var cell = this.ActiveSheet.Cells[row + i, column + j];
                    var cellFormat = new CellFormat
                                     {
                                         Width = this.ActiveSheet.Column(column + j).Width, Height = this.ActiveSheet.Row(row + i).Height
                                     };

                    var font = cell.Style.Font;
                    cellFormat.Font = new CellFont
                                      {
                                          IsBold = font.Bold,
                                          IsItalic = font.Italic,
                                          Size = font.Size,
                                          Color = font.Color.Rgb,
                                          Name = font.Name
                                      };
                    result[i, j] = cellFormat;
                }
            }

            return result;
        }

        private bool IsFormula(string formula) => !string.IsNullOrWhiteSpace(formula) && formula.StartsWith("=");

        private object[,] GetRangeContents(int row, int column, int height, int width, Func<ExcelRange, object> extractorFunc)
        {
            var rez = new object[height, width];
            var worksheet = this.ActiveSheet;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    rez[i, j] = extractorFunc(worksheet.Cells[row + i, column + j]);
                }
            }

            return rez;
        }

        private void RunMacroInternal(string macroName, params object[] parameters)
        {
            if (this.package.Workbook.VbaProject == null)
            {
                this.package.Workbook.CreateVBAProject();
            }

            var module = this.package.Workbook.VbaProject.Modules["ThisWorkbook"];
            if (module != null)
            {
                var sb = new StringBuilder();

                sb.AppendLine("Private Sub Workbook_Open()");
                sb.AppendLine("Call " + macroName + "(" + string.Join(" , ", parameters.Where(p => p != null).ToArray()) + ")");
                sb.AppendLine("End Sub");

                module.Code = sb.ToString();
            }
        }

        private void CopyRows(int row, int height, int delta)
        {
            var sourceStartRow = row;
            var sourceEndRow = sourceStartRow + height - 1;

            var destinationStartRow = sourceStartRow + (height * (delta + 1));
            var destinationEndRow = destinationStartRow + height - 1;

            var startColumn = this.ActiveSheet.Dimension.Start.Column;
            var endColumn = this.ActiveSheet.Dimension.End.Column;

            this.ActiveSheet.Cells[sourceStartRow, startColumn, sourceEndRow, endColumn]
                .Copy(this.ActiveSheet.Cells[destinationStartRow, startColumn, destinationEndRow, endColumn]);
        }

        private void SetFields(ExcelWorksheet worksheet, IList<ExcelField> setFieldInfos)
        {
            if (null == worksheet || null == setFieldInfos)
            {
                return;
            }

            foreach (var setFieldInfo in setFieldInfos)
            {
                var range = worksheet.Cells[setFieldInfo.Row, setFieldInfo.Column];
                range.Value = setFieldInfo.Value;
                if (null == setFieldInfo.Options)
                {
                    continue;
                }

                range.Style.Font.Bold = setFieldInfo.Options.Bold;
                range.Style.Font.Size = setFieldInfo.Options.FontSize;
                if (setFieldInfo.Options.ColorIndex > 0)
                {
                    range.Style.Fill.BackgroundColor.Indexed = setFieldInfo.Options.ColorIndex;
                }
                else
                {
                    range.Style.Fill.BackgroundColor.SetColor(setFieldInfo.Options.BackgroundColor);
                }

                range.Style.Border.BorderAround(ExcelBorderStyle.Hair);
            }
        }
    }
}
