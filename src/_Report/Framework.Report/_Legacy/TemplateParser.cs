using System;
using System.Collections.Generic;
using System.Text;
using Framework.Report.Template;

namespace Framework.Report
{
    public abstract class TemplateParser
    {
        public abstract IList<ReportTemplate> Parse();
    }
}
