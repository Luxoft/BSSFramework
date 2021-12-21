using System;
using System.Drawing;
using System.Text.RegularExpressions;

using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public class ExcelDesignFormula
    {
        private const string ValueTemplate = "#value";

        private const string HostTemplate = "#host";

        private static readonly Regex HyperLinkPattern = new Regex("hyperlink\\(\"(.+)\",\\s+\"(.+)\"\\)", RegexOptions.IgnoreCase);

        public static ExcelDesignFormula CreateHyperlink(string formula)
        {
            return new ExcelDesignFormula(formula, ExcelDesignFormulaType.HyperLink);
        }

        public static ExcelDesignFormula CreateExcelFormula(string formula)
        {
            return new ExcelDesignFormula(formula, ExcelDesignFormulaType.Formula);
        }

        private readonly string formula;

        private readonly ExcelDesignFormulaType formulaType;

        private ExcelDesignFormula(string formula, ExcelDesignFormulaType formulaType)
        {
            this.formula = formula;
            this.formulaType = formulaType;
        }

        public void ApplyFormula(ExcelRange range)
        {
            switch (this.formulaType)
            {
                case ExcelDesignFormulaType.HyperLink:
                    this.RenderHyperLink(range);
                    break;
                case ExcelDesignFormulaType.Formula:
                    this.RenderFormula(range);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RenderFormula(ExcelRange range)
        {
            this.Render(range, (rangeBase, value) => rangeBase.Formula = value);
        }

        private void Render(ExcelRange range, Action<ExcelRangeBase, string> applyFunc)
        {
            if (this.formula.Contains(HostTemplate) || this.formula.Contains(ValueTemplate))
            {
                var match = HyperLinkPattern.Match(this.formula);
                var hostEvaluated = this.formula;
                if (match.Success)
                {
                    hostEvaluated = Regex.Replace(match.Groups[1].Value, HostTemplate, Environment.MachineName, RegexOptions.IgnoreCase);
                }

                for (int q = 0; q < range.Rows; q++)
                {
                    using (var rowRange = range.Offset(q, 0, 1, 1))
                    {
                        var valueEvaluated = Regex.Replace(hostEvaluated, ValueTemplate, rowRange.Value?.ToString() ?? string.Empty, RegexOptions.IgnoreCase);

                        applyFunc(rowRange, valueEvaluated);
                    }
                }
            }
            else
            {
                applyFunc(range, this.formula);
            }
        }

        private void RenderHyperLink(ExcelRange range)
        {
            this.Render(range, (rangeBase, value) => rangeBase.Hyperlink = new Uri(value));
        }

        enum ExcelDesignFormulaType
        {
            HyperLink,
            Formula
        }
    }
}
