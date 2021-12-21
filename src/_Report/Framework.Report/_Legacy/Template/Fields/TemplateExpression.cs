using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Report.Template
{
    public class TemplateExpression
    {
        string expression;

        public string Expression
        {
            get { return this.expression; }
        }

        int originalIndex;
        public int OriginalIndex
        {
            get { return this.originalIndex; }
        }

        string originalString;

        public string OriginalString
        {
            get { return this.originalString; }
        }

        public TemplateExpression(string expression, int originalIndex, string originalString)
        {
            this.expression = expression;
            this.originalIndex = originalIndex;
            this.originalString = originalString;
        }

    }
}
