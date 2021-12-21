using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Framework.Report.Template
{
    public class StaticTemplateField: TemplateFieldBase
    {
        public StaticTemplateField(Point location, object originalValue)
            : base(location, originalValue)
        {
        }
    }
}
