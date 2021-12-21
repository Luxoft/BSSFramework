using System.Collections.Generic;
using System.IO;

using Framework.Core;

namespace Framework.Report.Spring.Text
{
    public class TextTemplateEvaluator : FileTemplateEvaluator, ITemplateEvaluator<string>
    {
        public TextTemplateEvaluator(string extension)
            : base(extension)
        {

        }

        protected override FileInfo EvaluateInternal(FileInfo template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false)
        {
            var textResult = this.Evaluate(File.ReadAllText(template.FullName), rootObject, variables, throwEvaluateException);

            var tempFileName = FileHelper.GetTempFileName(template.GetPureExtension());

            File.WriteAllText(tempFileName, textResult);

            return new FileInfo(tempFileName);
        }

        public string Evaluate(string template, object rootObject = null, IReadOnlyDictionary<string, object> variables = null, bool throwEvaluateException = false)
        {
            return new TextReportBuilder { RootObject = rootObject, Variables = variables }.Execute(template, throwEvaluateException);
        }


        public static readonly TextTemplateEvaluator Default = new TextTemplateEvaluator(null);
    }
}