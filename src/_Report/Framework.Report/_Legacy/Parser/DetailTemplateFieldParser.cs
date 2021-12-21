using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Framework.Report.Template;
using System.Drawing;

namespace Framework.Report.Parser
{
    public class DetailTemplateFieldParser : TemplateStringParser
    {
        private const string pattern = @"##{(?<dir>ByRow|ByCol)\(#(?<name>[^#]*?)(#.*?)?\),\s*(?<expr>[^}]*)}";
        private static readonly Regex regex = new Regex(pattern);

        public DetailTemplateFieldParser(ReportTemplate report)
            : base(report)
        {

        }
        public DetailTemplateFieldParser(TemplateObjectParser next)
            : base(next)
        {
        }
        protected override bool ParseInternal(string expression, Point point)
        {
            Match result = regex.Match(expression);
            if (!result.Success)
            {
                return false;
            }
            IList<TemplateExpression> expressions = new List<TemplateExpression>();
            string sectionName = string.Empty;
            Match current = result;
            ShiftDirection shiftDirection = current.Groups["dir"].Captures[0].ToString().Equals("ByRow")
                    ? ShiftDirection.TopDown : ShiftDirection.LeftRight;
            while (current.Success)
            {
                sectionName = current.Groups["name"].Captures[0].Value;
                expressions.Add(
                    new TemplateExpression(
                        current.Groups["expr"].Captures[0].Value,
                        current.Index,
                        current.Value));
                current = current.NextMatch();
            }
            TemplateSection section = this.Report.GetSection(sectionName);
            if (null == section)
            {

                section = new TemplateDetailsSection(this.Report, shiftDirection, sectionName);
                this.Report.SubSections.Add(section);
            }
            section.TemplateFields.Add(new TemplateField(section, point, expression, expressions));
            return true;
        }
    }
}
