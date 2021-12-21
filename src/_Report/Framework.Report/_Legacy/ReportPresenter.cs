using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Framework.Report.TemplateEvaluationResult;
using Framework.Report.Template;

namespace Framework.Report
{
    public abstract class ReportPresenter
    {
        public abstract void CreateView(ReportTemplate report);
    }
}
