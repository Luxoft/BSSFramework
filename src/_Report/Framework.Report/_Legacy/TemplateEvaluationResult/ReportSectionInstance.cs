using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Report.Template;
using System.Drawing;
using Framework.Core;

namespace Framework.Report.TemplateEvaluationResult
{
    public class ReportSectionInstance
    {
        TemplateSection template;
        Rectangle range = Rectangle.Empty;
        ReportSectionInstance parent;
        private readonly int index;
        List<ReportInstanceField> fields = new List<ReportInstanceField>();
        List<ReportSectionInstance> subSections = new List<ReportSectionInstance>();
        bool prepared = false;
        internal Dictionary<TemplateFieldBase, object> map = new Dictionary<TemplateFieldBase, object>();

        public ReportSectionInstance(TemplateSection template, ReportSectionInstance parent, int index)
        {
            this.template = template;
            this.parent = parent;
            this.index = index;
            if (null != parent)
                parent.SubSections.Add(this);
        }
        public IEnumerable<ReportSectionInstance> Parallel
        {
            get
            {
                return this.parent.Maybe(z => z.SubSections.Where(w => w.Template == this.template.Parallel))
                    .IfDefault(new List<ReportSectionInstance>());
            }
        }
        public int Index
        {
            get { return this.index; }
        }

        public TemplateSection Template
        {
            get { return this.template; }
        }

        public ReportSectionInstance Parent
        {
            get { return this.parent; }
        }

        public Rectangle Range
        {
            get
            {
                if (!this.prepared)
                {
                    this.range = CoordinatesUtil.GetContainingRectangle(
                    ListUtils<ReportInstanceField>.GetPropertyValueList<Point>(this.AllFields,
                    field => field.Location));
                }
                return this.range;
            }
        }

        public List<ReportInstanceField> Fields
        {
            get { return this.fields; }
        }
        public List<ReportSectionInstance> SubSections
        {
            get { return this.subSections; }
        }

        public List<ReportInstanceField> AllFields
        {
            get
            {
                List<ReportInstanceField> result = new List<ReportInstanceField>(this.fields);
                foreach (ReportSectionInstance subsection in this.SubSections)
                {
                    result.AddRange(subsection.AllFields);
                }
                return result;
            }
        }
        public IList<ReportInstanceField> GetFieldsInTemplateRectangle(Rectangle range)
        {
            List<ReportInstanceField> result = new List<ReportInstanceField>();
            foreach (ReportInstanceField field in this.Fields)
            {
                if (range.Contains(field.Template.Location))
                    result.Add(field);
            }
            if (null != this.parent)
            {
                result.AddRange(this.parent.GetFieldsInTemplateRectangle(range));
            }
            return result;
        }
        public void ShiftParents()
        {
            this.ShiftParents(this.Range);
        }
        private void ShiftParents(Rectangle range)
        {
            foreach (ReportInstanceField field in this.parent.Fields)
            {
                if (((this.template.ShiftDirection != ShiftDirection.NewPage)&&
                    ((this.template.ShiftDirection == ShiftDirection.TopDown && field.Template.Location.Y > range.Bottom)
                        || (this.template.ShiftDirection == ShiftDirection.LeftRight && field.Template.Location.X > range.Right ))))
                {

                }
            }
        }
        public IList<ReportSectionInstance> GetInstancesByTemplate(TemplateSection template)
        {
            List<ReportSectionInstance> result = new List<ReportSectionInstance>();
            foreach (ReportSectionInstance subsection in this.SubSections)
            {
                if (subsection.Template == template)
                    result.Add(subsection);
            }
            return result;
        }
        public void Prepare()
        {
            this.range = CoordinatesUtil.GetContainingRectangle(
                ListUtils<ReportInstanceField>.GetPropertyValueList<Point>(this.AllFields,
                delegate(ReportInstanceField field) { return field.Location; }));
            this.prepared = true;
            foreach (ReportSectionInstance section in this.SubSections)
            {
                section.Prepare();
            }
        }

        public object GetValuebyTemplateField(TemplateFieldBase a_obj)
        {
            return (this.map.ContainsKey(a_obj) ? this.map[a_obj] : null);
        }

        private ReportSectionInstance Previous
        {
            get
            {
                return (this.index == 0)
                           ? this.Template.Previous
                                 .Maybe(z => z.SubSections.LastOrDefault()
                                                 .Maybe(w => w.Instances.LastOrDefault()))
                           : this.parent.SubSections[this.index - 1];

            }
        }
        public int GetVerticalShiftFromTemplate()
        {
            return this.Previous.Maybe(z => this.GetVerticalShiftFromTemplate()) + this.index *this.template.Range.Height;
        }

    }
}