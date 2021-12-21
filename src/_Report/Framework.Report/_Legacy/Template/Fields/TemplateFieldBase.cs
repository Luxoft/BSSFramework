using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Framework.Report.Template
{
    public class TemplateFieldBase
    {
        readonly Point location;
        readonly object originalValue;
        public TemplateFieldBase(Point location, object originalValue)
        {
            this.location = location;
            this.originalValue = originalValue;
        }
        public object OriginalValue
        {
            get { return this.originalValue; }
        }
        public string OriginalString
        {
            get { return this.originalValue as string; }
        }
        public Point Location
        {
            get { return this.location; }
        }

        public virtual string GetEvaluationString()
        {
            return this.OriginalString;
        }
    }
}
