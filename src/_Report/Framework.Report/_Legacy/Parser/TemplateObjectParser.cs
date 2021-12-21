using System;
using System.Collections.Generic;
using System.Text;
using Framework.Report.Template;
using System.Drawing;

namespace Framework.Report.Parser
{
    public abstract class TemplateObjectParser
    {
        TemplateObjectParser next;
        ReportTemplate report;
        protected ReportTemplate Report
        {
            get { return this.report; }
            set { this.report = value; }
        }
        public TemplateObjectParser(ReportTemplate report)
        {
            this.report = report;
        }
        public TemplateObjectParser(TemplateObjectParser next)
        {
            this.report = next.report;
            this.next = next;
        }
        public bool Parse(object obj, Point point)
        {
            if (null == obj) return false;
            return this.ParseInternal(obj, point) || ((null != this.next) && this.next.Parse(obj, point));
        }
        protected abstract bool ParseInternal(object expression, Point point);
    }
}
