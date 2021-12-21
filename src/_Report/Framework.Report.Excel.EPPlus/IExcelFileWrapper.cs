using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework.Report.Excel.EPPlus
{
    /// <summary>
    /// Should be moved to some Core project and shared between Framework.Report.Excel.Engine and Framework.Report.Excel.EPPlus
    /// once this EPPlus technique adopted
    /// </summary>
    public interface IExcelFileWrapper : IDisposable
    {
        string ActiveWorkSheetName { get; set; }

        void Open(byte[] excelFileBytes);

        void SetPassword(string password);

        void RunMacro(string macroName);

        void RunMacro(string macroName, object param1);

        void RunMacro2(string macroName, bool disableScreenUpdating);

        void SetRange(int row, int column, object[,] values);

        void SetRange(int row, int column, string worksheetName, object[,] values);

        void ShiftRows(int row, int height, int times);

        object[,] GetRangeFormulas(int row, int column, int height, int width);

        object[,] GetWorkingRangeFormulas();

        object[,] GetWorkingRangeValues();

        void Save();

        void SaveAs(string name);

        void SaveAsPdf(string name);

        void SaveToStream(Stream stream);

        IList<string> GetSheetsNamesExcludePivot();

        void AutoFitColumns();
    }
}
