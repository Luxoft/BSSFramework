using System;
using System.Collections.Generic;
using System.IO;

using Framework.Report.Template;

namespace Framework.Report.Spring.Text
{
    public class TextReportBuilder : FileReportBuilderBase
    {
        #region IFileTemplateEvaluator Members

        public string Execute(string templateString, bool throwEvaluateException = false)
        {
            TextTemplateParser parser = new TextTemplateParser(templateString);
            IList<ReportTemplate> templates = parser.Parse();
            using (TextReportPresenter presenter = new TextReportPresenter(this.evaluator))
            {
                foreach (ReportTemplate template in templates)
                {
                    this.evaluator.Evaluate(template, throwEvaluateException);
                    presenter.CreateView(template);
                }
                return presenter.Result[0];
            }
        }

        public override void Execute(byte[] fileBytes, ref string targetFileName, bool evaluateExceptionRaise = false)
        {
            string textLine;
            using (StreamReader streamReader = new StreamReader(new MemoryStream(fileBytes)))
            {
                textLine = streamReader.ReadToEnd();
            }

            string resultString = this.Execute(textLine);

            using (StreamWriter sw = new StreamWriter(File.Open(targetFileName, FileMode.Create, FileAccess.Write)))
            {
                sw.WriteLine(resultString);
            }
        }

        #endregion
    }
}
