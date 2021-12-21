using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Framework.Report.TemplateEvaluationResult;

namespace Framework.Report.Template
{
    public enum ShiftDirection {TopDown, LeftRight, NewPage, None} ;
    public class ReportTemplate : TemplateSection
    {
        public override string Name
        {
            get { return "__Report"; }
        }

        public ReportTemplate(ShiftDirection shiftDirection)
            : base(null, shiftDirection)
        { }
        public override ReportSectionInstance CreateReportSection(ReportSectionInstance parent, int index)
        {
            return new ReportInstance(this);
        }
    }
}
