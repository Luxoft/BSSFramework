using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Framework.Report.Template
{
    public class TemplateField : TemplateFieldBase
    {
        IList<TemplateExpression> expressions;

        public IList<TemplateExpression> Expressions
        {
            get { return this.expressions; }
        }

        TemplateSection section;

        public TemplateSection Section
        {
            get { return this.section; }
        }
        public Point SectionLocation
        {
            get { return new Point(this.Location.X - this.section.Range.X, this.Location.Y - this.section.Range.Y); }
        }
        public TemplateField(TemplateSection section, Point location, string originalString, IList<TemplateExpression> expressions)
            : base(location, originalString)
        {
            this.expressions = expressions;
            this.section = section;
        }
    }
}
