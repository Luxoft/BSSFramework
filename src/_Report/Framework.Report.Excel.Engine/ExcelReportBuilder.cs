using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Framework.Report.Excel.EPPlus;
using Framework.Report.Spring;
using Framework.Report.Template;

namespace Framework.Report.Excel.Engine
{
    /// <summary>
    /// Copy of Common.Reporting.Excel.ExcelReportBuilder adjusted to work with ExcelEPPlusFileWrapper
    /// </summary>
    public class ExcelReportBuilder : FileReportBuilderBase
    {
        private static object thisLock = new object();
        private static Func<string, IExcelFileWrapper> fileWrapperFactory;
        private string formatMacroName;

        static ExcelReportBuilder()
        {
            fileWrapperFactory = targetCopyFileName => new ExcelEPPlusFileWrapper(targetCopyFileName);
        }

        public ExcelReportBuilder()
            : base()
        {

        }

        public string FormatMacroName
        {
            get { return this.formatMacroName; }
            set { this.formatMacroName = value; }
        }

        public static void ConvertToPdf(byte[] excelFileBytes, ref string targetFileName)
        {
            string pdfFileName = targetFileName + ".xlsx";
            using (var excel = fileWrapperFactory(pdfFileName))
            {
                excel.Open(excelFileBytes);
                excel.SaveAsPdf(pdfFileName);
            }

            targetFileName = pdfFileName;
        }

        public static void ConvertToHtml(byte[] excelFileBytes, ref string targetFileName)
        {
            ConvertToPdf(excelFileBytes, ref targetFileName);
        }

        [Obsolete("Please use Execute and ConvertToPdf methods")]
        public void ExecuteAsPdf(byte[] excelFileBytes, ref string targetFileName)
        {
            string pdfFileName = targetFileName + ".xlsx";
            this.ExecuteInternal(excelFileBytes, ref targetFileName, string.Empty, x => { x.SaveAs(pdfFileName); });
            targetFileName = pdfFileName;
        }

        public void ExecuteToXlsx(byte[] excelFileBytes, ref string targetFileName)
        {
            this.ExecuteInternal(excelFileBytes, ref targetFileName, string.Empty, x => x.Save());
        }

        public override void Execute(byte[] excelFileBytes, ref string targetFileName, bool evaluateExceptionRaise = false)
        {
            this.Execute(excelFileBytes, ref targetFileName, string.Empty, evaluateExceptionRaise);
        }

        public void Execute(byte[] excelFileBytes, ref string targetFileName, string password, bool evaluateExceptionRaise = false)
        {
            this.ExecuteInternal(excelFileBytes, ref targetFileName, password, x => x.Save(), evaluateExceptionRaise);
        }

        public MemoryStream Execute(byte[] excelFileBytes)
        {
            MemoryStream rez = new MemoryStream(1024 * 50);
            string fileName = "empty";
            this.ExecuteInternal(excelFileBytes, ref fileName, null, x => x.SaveToStream(rez), false);
            return rez;
        }

        internal void ExecuteInternal(
            byte[] excelFileBytes,
            ref string targetFileName,
            string password,
            Action<IExcelFileWrapper> saveAction,
            bool evaluateExceptionRaise = false)
        {
            var previousCultureInfo = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                using (var excel = fileWrapperFactory(targetFileName))
                {
                    excel.Open(excelFileBytes);
                    ExcelTemplateParser parser = new ExcelTemplateParser(excel);
                    IList<ReportTemplate> templates = parser.Parse();
                    ExcelReportPresenter presenter = new ExcelReportPresenter(excel, this.evaluator);
                    IList<Exception> evaluationExceptions = new List<Exception>();
                    foreach (ExcelReportTemplate template in templates)
                    {
                        excel.ActiveWorkSheetName = template.WorksheetName;
                        this.evaluator.Evaluate(template, evaluateExceptionRaise);
                        presenter.CreateView(template);
                    }

                    if (!string.IsNullOrEmpty(password))
                    {
                        excel.SetPassword(password);
                    }

                    if (!string.IsNullOrEmpty(this.FormatMacroName))
                    {
                        excel.RunMacro(this.FormatMacroName);
                    }

                    excel.AutoFitColumns();

                    lock (thisLock)
                    {
                        saveAction(excel);
                    }
                }
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previousCultureInfo;
            }
        }
    }
}
