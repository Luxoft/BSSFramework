using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Framework.Report.Template;

namespace Framework.Report.TemplateEvaluationResult
{
    public class ReportInstanceField
    {
        Point location;
        public Point Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
        public Point SectionLocation
        {
            get { return this.location - (Size)this.parent.Range.Location; }
        }
        object value;
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public string StringValue
        {
            get { return null == this.value ? string.Empty : this.value.ToString(); }
        }
        ReportSectionInstance parent;
        TemplateFieldBase template;

        public TemplateFieldBase Template
        {
            get { return this.template; }
        }
        public ReportInstanceField(ReportSectionInstance parent, TemplateFieldBase templateField, Size shift, object value)
        {
            this.template = templateField;
            this.location = templateField.Location + shift;
            this.parent = parent;
            this.value = value;

            parent.map.Add(this.template, this.value);
        }
        public ReportInstanceField CloneAndShift(ReportSectionInstance newParent, Size shift)
        {
            return new ReportInstanceField(newParent, this.Template, shift, this.Value);
        }
    }

}
