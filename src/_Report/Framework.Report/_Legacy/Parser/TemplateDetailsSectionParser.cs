using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Framework.Report.Template;
using System.Drawing;

namespace Framework.Report.Parser
{
    public class TemplateDetailsSectionParser : TemplateStringParser
    {
        private const string pattern = @"##{(?<dir>ByRow|ByCol)\(#(?<name>[^#]+)#(?<parent>[^#]*)\),\s*(?<expr>[^}]*)}";
        private static readonly Regex regex = new Regex(pattern);
        public TemplateDetailsSectionParser(ReportTemplate report)
            : base(report)
        {

        }
        public TemplateDetailsSectionParser(TemplateObjectParser next)
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
            Match current = result;

            string parentName = current.Groups["parent"].Captures[0].Value;
            string sectionName = result.Groups["name"].Captures[0].Value;
            TemplateSection parentSection = string.IsNullOrEmpty(parentName) ? this.Report : this.Report.GetSection(parentName);
            if (null == parentSection)
                parentSection = this.Report;
            if (null == parentSection.GetSection(sectionName))
            {
                ShiftDirection dir =
                    current.Groups["dir"].Captures[0].ToString().Equals("ByRow")
                    ? ShiftDirection.TopDown : ShiftDirection.LeftRight;
                TemplateDetailsSection section = new TemplateDetailsSection(parentSection, dir, sectionName);
                parentSection.SubSections.Add(section);
            }
            return false;
        }
    }
}
