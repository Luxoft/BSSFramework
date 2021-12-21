using System;
using System.Collections.Generic;
using System.Text;
using Framework.Report.Template;
using System.Drawing;

namespace Framework.Report.Parser
{
    public abstract class TemplateStringParser : TemplateObjectParser
    {
        public TemplateStringParser(ReportTemplate report)
            : base(report)
        {
        }
        public TemplateStringParser(TemplateObjectParser next)
            : base(next)
        {
        }
        protected override bool ParseInternal(object obj, Point point)
        {
            string expression = obj as string;
            return (null == expression) || this.ParseInternal(expression, point);
        }
        protected abstract bool ParseInternal(string expression, Point point);
    }
}
