using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Framework.Core;
using Framework.Report.TemplateEvaluationResult;
using Framework.Report.Template;
using System.Drawing;

namespace Framework.Report.Spring.Text
{
    public class TextReportPresenter : ReportPresenter, IDisposable
    {
        private readonly IShiftEventSource shiftSource;
        IList<string> result;

        public IList<string> Result
        {
            get { return this.result; }
        }
        public TextReportPresenter(IShiftEventSource shiftSource)
        {
            this.shiftSource = shiftSource;
            shiftSource.OnShift += this.shiftSource_OnShift;
        }

        public override void CreateView(ReportTemplate template)
        {
            this.result = new List<string>();
            foreach (ReportInstance report in template.Instances)
            {
                report.Prepare();
                string[] res = new string[report.Range.Height + 2];
                foreach (ReportInstanceField field in report.AllFields)
                {
                    string curValue = res[field.Location.Y];
                    if (null != curValue)
                        res[field.Location.Y] = curValue.Replace(field.Template.OriginalString, field.StringValue);
                    else
                        res[field.Location.Y] = field.StringValue;
                }
                this.result.Add(string.Join("\r\n", res.Where(z => z != null).ToArray()));
            }
        }
        void shiftSource_OnShift(object sender, EventArgs<ShiftArgs> args)
        {
            ShiftArgs shift = args.Data;
            if (shift.ShiftDirection == ShiftDirection.TopDown)
            {
                if (shift.ReverseShift)
                {
                    args.Data.TotalShift -= new Size(0, shift.TemplateRange.Height + 1);
                }
                else
                {
                    args.Data.TotalShift += new Size(0, shift.TemplateRange.Height + 1);
                }
            }
        }

        public void Dispose()
        {
            this.shiftSource.OnShift -= this.shiftSource_OnShift;
        }
    }
}
