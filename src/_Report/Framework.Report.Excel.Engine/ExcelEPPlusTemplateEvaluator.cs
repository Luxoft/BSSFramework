using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Report.Excel.Engine
{
    public class ExcelEPPlusTemplateEvaluator : FileTemplateEvaluator
    {
        public ExcelEPPlusTemplateEvaluator(string extension)
            : base(extension)
        {

        }

        protected override FileInfo EvaluateInternal([NotNull] FileInfo template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));

            var baseTempFileName = FileHelper.GetTempFileName(template.GetPureExtension());

            var tempFileName = baseTempFileName;

            var previousCultureInfo = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                new ExcelReportBuilder { RootObject = rootObject, Variables = variables }.Execute(File.ReadAllBytes(template.FullName), ref tempFileName, throwEvaluateException);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previousCultureInfo;
            }

            try
            {
                return new FileInfo(tempFileName);
            }
            finally
            {
                if (tempFileName != baseTempFileName)
                {
                    File.Delete(baseTempFileName);
                }
            }
        }
    }
}
