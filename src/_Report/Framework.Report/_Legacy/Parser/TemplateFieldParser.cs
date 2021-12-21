using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Framework.Report.Template;
using System.Drawing;

namespace Framework.Report.Parser
{
    public class TemplateFieldParser : TemplateStringParser
    {
        const string pattern = @"(?<template>#{(?<expr>[^}]*)})";
        private static readonly Regex regex = new Regex(pattern);

        public TemplateFieldParser(ReportTemplate report)
            : base(report)
        {

        }
        public TemplateFieldParser(TemplateObjectParser next)
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
            Match current = result;
            while (current.Success)
            {

                expressions.Add(
                    new TemplateExpression(
                        current.Groups["expr"].Captures[0].Value,
                        current.Index,
                        current.Value));
                current = current.NextMatch();
            }
            this.Report.TemplateFields.Add(new TemplateField(this.Report, point, expression, expressions));
            return true;
        }
    }
}
