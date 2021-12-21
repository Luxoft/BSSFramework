using System;
using System.Collections.Generic;
using System.Text;
using Framework.Report.Template;
using System.Drawing;

namespace Framework.Report.Parser
{
    public class StaticTemplateFieldParser : TemplateObjectParser
    {
        public StaticTemplateFieldParser(ReportTemplate report)
            : base(report)
        {

        }
        public StaticTemplateFieldParser(TemplateObjectParser next)
            : base(next)
        {
        }
        protected override bool ParseInternal(object value, Point point)
        {
            if ((null == value) || (string.IsNullOrEmpty(value.ToString())) || value.ToString().StartsWith("=")) return false;
            this.Report.TemplateFields.Add(new StaticTemplateField(point, value));
            return true;
        }
    }
}
