using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Framework.Report.TemplateEvaluationResult;

namespace Framework.Report.Template
{


    public class TemplateDetailsSection : TemplateSection
    {

        //public Point MakeShift(int shift)
        //{
        //    return (direction == Direction.TopDown) ? new Point(0, shift) : new Point(shift, 0);
        //}
        //public override int GetDetailsSize()
        //{
        //    return (direction == Direction.TopDown) ? this.Range.Size.Height : this.Range.Size.Width;
        //}

        private string getCollectionExpression;

        public string GetCollectionExpression
        {
            get { return this.getCollectionExpression; }
        }

        public TemplateDetailsSection(TemplateSection parent, ShiftDirection shiftDirection, string getCollectionExpression)
            : base(parent, shiftDirection)
        {
            this.getCollectionExpression = getCollectionExpression;
        }

        public override string Name
        {
            get { return this.getCollectionExpression; }
        }

        public override ReportSectionInstance CreateReportSection(ReportSectionInstance parent, int index)
        {
            return new ReportSectionInstance(this, parent, index);
        }
        public string GetColectionEvaluationString(string contextName)
        {
            if (this.getCollectionExpression.Trim().StartsWith("{") || this.getCollectionExpression.Trim().StartsWith(contextName))
            {
                return this.getCollectionExpression;
            }
            return contextName + "." + this.getCollectionExpression;
        }
    }
}
