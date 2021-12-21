using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Framework.Report.TemplateEvaluationResult;

namespace Framework.Report.Template
{
    public abstract class TemplateSection
    {
        private bool prepared;
        Rectangle range;
        private int index;
        ShiftDirection shiftDirection;
        List<TemplateFieldBase> templateFields = new List<TemplateFieldBase>();
        List<TemplateSection> subSections = new List<TemplateSection>();

        List<ReportSectionInstance> instances = new List<ReportSectionInstance>();



        TemplateSection parent;
        private TemplateSection parallel;
        private string varName;

        public TemplateSection(TemplateSection parent, ShiftDirection shiftDirection)
        {
            this.parent = parent;
            this.shiftDirection = shiftDirection;
            this.varName = "section" + Guid.NewGuid().ToString("N");

        }
        public List<ReportSectionInstance> Instances
        {
            get { return this.instances; }
        }
        public TemplateSection Parent
        {
            get { return this.parent; }
        }
        public TemplateSection Previous
        {
            get
            {
                return (this.index == 0) ? this.parent : this.parent.SubSections[this.index - 1];
            }
        }
        public TemplateSection Parallel
        {
            get
            {
                return this.parallel;
            }
        }

        public List<TemplateSection> SubSections
        {
            get { return this.subSections; }
        }
        public Rectangle Range
        {
            get { return this.range; }
            protected set { this.range = value; }
        }
        //public virtual IList<StaticTemplateField> StaticFields
        //{
        //    get { return staticFields; }
        //}
        public IList<TemplateFieldBase> TemplateFields
        {
            get { return this.prepared ? (IList<TemplateFieldBase>)this.templateFields.AsReadOnly () : (IList<TemplateFieldBase>)this.templateFields; }
        }

        public TemplateSection GetSection (string name)
        {
            TemplateSection result = null;
            foreach (TemplateSection section in this.SubSections)
            {
                if (name == section.Name)
                    return section;
                else
                    result = section.GetSection (name);
            }
            return result;
        }

        public virtual void Prepare(int index)
        {
            this.Range = CoordinatesUtil.GetContainingRectangle(this.templateFields.Select(z => z.Location).ToList());
            this.index = index;
            var count = 0;
            foreach (TemplateSection section in this.SubSections)
            {
                section.Prepare(count);
                count++;
            }
            foreach (TemplateSection section in this.SubSections)
            {
                var sec = section;
                section.parallel = this.SubSections.FirstOrDefault(z => z != sec && z.Range.Y == sec.Range.Y);
            }

            this.prepared = true;
        }
        public abstract string Name
        {
            get;
        }
        public string VarName
        {
            get { return this.varName; }
        }
        public abstract ReportSectionInstance CreateReportSection(ReportSectionInstance parent, int index);

        public ShiftDirection ShiftDirection
        {
            get { return this.shiftDirection; }
        }

        public int Index
        {
            get { return this.index; }
        }

        public IList<TemplateFieldBase> GetIntersectedParentFields()
        {
            return this.GetAncestorsFieldsInRectangle(this.Range);
        }
        public IList<TemplateFieldBase> GetAncestorsFieldsInRectangle(Rectangle range)
        {
            IList<TemplateFieldBase> result = new List<TemplateFieldBase>();
            foreach (TemplateFieldBase field in this.parent.TemplateFields)
            {
                if (range.Contains(field.Location))
                    result.Add(field);
            }
            return result;
        }


        public bool ShiftAffectsField (TemplateFieldBase field )
        {
            bool result = ((this.shiftDirection != ShiftDirection.NewPage)&&
                    ((this.shiftDirection == ShiftDirection.TopDown && field.Location.Y > this.range.Bottom)
                        || (this.shiftDirection == ShiftDirection.LeftRight && field.Location.X > this.range.Right )));
            if ((!result)&&(this.SubSections.Count > 0))
            {
                bool affectedBySubsections = true;
                foreach (TemplateSection subSection in this.SubSections)
                {
                    affectedBySubsections &= subSection.ShiftAffectsField(field);
                }
                result |= affectedBySubsections;
            }
            return result;
        }
        public void ClearInstances()
        {
            this.instances.Clear();
            foreach (TemplateSection subSection in this.SubSections)
            {
                subSection.ClearInstances();
            }
        }

    }
}
