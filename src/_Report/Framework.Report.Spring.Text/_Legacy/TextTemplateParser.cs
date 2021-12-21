using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Framework.Report.Parser;
using Framework.Report.Template;

namespace Framework.Report.Spring.Text
{
    public class TextTemplateParser : TemplateParser
    {
        string source;
        public TextTemplateParser(string source)
        {
            this.source = source;
        }
        public override IList<ReportTemplate> Parse()
        {
            ReportTemplate result = new ReportTemplate(ShiftDirection.None);
            TemplateObjectParser parsingChain =
                 new TemplateDetailsSectionParser(
                     new DetailTemplateFieldParser(
                         new TemplateFieldParser(
                             new StaticTemplateFieldParser(result))));
            string[] lines = this.source.Replace("\r\n", "\n").Replace("\r", "\n")
                .Split(new string[] { "\n" }, StringSplitOptions.None)
                .Select(z => z ?? string.Empty).ToArray();
            for(int i = 0; i < lines.Length; i++)
            {
                parsingChain.Parse(lines[i], new Point(0, i));
            }
            return new ReportTemplate[] { result };
        }
    }
}
