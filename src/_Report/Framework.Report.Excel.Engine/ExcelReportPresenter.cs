using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Framework.Report.Excel.EPPlus;
using Framework.Report.Template;
using Framework.Report.TemplateEvaluationResult;

namespace Framework.Report.Excel.Engine
{
    /// <summary>
    /// Copy of Common.Reporting.Excel.ExcelReportPresenter adjusted to work with ExcelEPPlusFileWrapper
    /// </summary>
    public class ExcelReportPresenter : ReportPresenter
    {
        private readonly IExcelFileWrapper excel;

        private Dictionary<ReportSectionInstance, bool> parallelSectionsMap =
            new Dictionary<ReportSectionInstance, bool>();

        public ExcelReportPresenter(IExcelFileWrapper excel, IShiftEventSource shiftSource)
        {
            this.excel = excel;
            shiftSource.OnShift += this.ShiftSource_OnShift;
        }

        public override void CreateView(ReportTemplate template)
        {
            foreach (var report in template.Instances.Cast<ReportInstance>())
            {
                report.Prepare();
                this.MakeShifts(report);

                if (!report.Range.IsEmpty)
                {
                    this.excel.SetRange(
                        report.Range.Location.Y + 1,
                        report.Range.Location.X + 1,
                        this.GetReportValues(report));
                }
            }
        }

        private void ShiftSource_OnShift(object sender, EventArgs<ShiftArgs> args)
        {
            var shift = args.Data;
            if (shift.ShiftDirection == ShiftDirection.TopDown)
            {
                if (shift.ReverseShift)
                {
                    args.Data.TotalShift -= new Size(0, shift.TemplateRange.Height + 1);
                }
                else
                {
                    args.Data.TotalShift += new Size(0, shift.TemplateRange.Height + 1);
                }
            }

            if (shift.ShiftDirection == ShiftDirection.LeftRight)
            {
                if (shift.ReverseShift)
                {
                    args.Data.TotalShift -= new Size(shift.TemplateRange.Width + 1, 0);
                }
                else
                {
                    args.Data.TotalShift += new Size(shift.TemplateRange.Width + 1, 0);
                }
            }

            if (shift.ShiftDirection == ShiftDirection.NewPage)
            {
                args.Data.TotalShift = new Size(0, 0);
            }
        }

        private void MakeShiftForGroup(IReadOnlyCollection<ReportSectionInstance> group)
        {
            if (group.Count <= 1)
            {
                return;
            }

            var first = group.First();
            var count = first.Parallel.Count();
            if ((count > group.Count) || (count == group.Count && first.Parallel.First().Range.X > first.Range.X))
            {
                return;
            }

            var range = first.Range;
            this.excel.ShiftRows(range.Y + 1, first.Template.Range.Height + 1, group.Count - 1);
        }

        private void MakeShiftsForEmptyTemplates(ReportSectionInstance section)
        {
            var emptyTemplateSubSections = section.Template.SubSections.Where(z => z.Instances.All(i => i.Parent != section));
            var emptyTemplateSubSectionsList = emptyTemplateSubSections.ToList();
            foreach (var templateSection in emptyTemplateSubSections.Reverse())
            {
                if (null != templateSection.Parallel)
                {
                    if (emptyTemplateSubSectionsList.Contains(templateSection.Parallel))
                    {
                        emptyTemplateSubSectionsList.Remove(templateSection);
                    }
                    else
                    {
                        continue;
                    }
                }

                var startRow = section.Range.Y - section.Template.Range.Y + templateSection.Range.Y + 1;
                this.excel.ShiftRows(startRow, templateSection.Range.Height + 1, -1);
            }
        }

        private void MakeShifts(ReportSectionInstance section)
        {
            this.MakeShiftsForEmptyTemplates(section);
            var subsectionGroupByTemplate = section.SubSections.GroupBy(z => z.Template);
            foreach (var group in subsectionGroupByTemplate)
            {
                this.MakeShiftForGroup(group.ToList());
                foreach (var reportSectionInstance in group)
                {
                    this.MakeShifts(reportSectionInstance);
                }
            }

            if (null != section.Parallel)
            {
                this.parallelSectionsMap.Add(section, true);
            }
        }

        private bool IsFormula(string formula) => !string.IsNullOrWhiteSpace(formula);

        private object[,] GetReportValues(ReportInstance report)
        {
            var values = new object[report.Range.Height + 1, report.Range.Width + 1];
            foreach (var field in report.AllFields)
            {
                values[field.Location.Y - report.Range.Y, field.Location.X - report.Range.X] = field.Value;
            }

            var formulas = this.excel.GetRangeFormulas(report.Range.Y + 1, report.Range.X + 1, report.Range.Height + 1, report.Range.Width + 1);

            for (var y = 0; y < report.Range.Height + 1; y++)
            {
                for (var x = 0; x < report.Range.Width + 1; x++)
                {
                    var formula = formulas[y, x] as string;
                    if (this.IsFormula(formula))
                    {
                        values[y, x] = "=" + formula;
                    }
                }
            }

            return values;
        }
    }
}
