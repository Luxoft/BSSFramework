using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Drawing;
using Framework.Report.TemplateEvaluationResult;
using Framework.Report.Template;
using System.Collections;

using Framework.Core;

namespace Framework.Report
{
    public class ShiftArgs
    {
        ShiftDirection shiftDirection;
        public ShiftDirection ShiftDirection
        {
            get { return this.shiftDirection; }
        }
        bool reverseShift;

        public bool ReverseShift
        {
            get { return this.reverseShift; }
        }


        bool shiftAccomplished;

        public bool ShiftAccomplished
        {
            get { return this.shiftAccomplished; }
            set { this.shiftAccomplished = value; }
        }
        Size totalShift;

        public Size TotalShift
        {
            get { return this.totalShift; }
            set { this.totalShift = value; }
        }
        Rectangle templateRange;

        public Rectangle TemplateRange
        {
            get { return this.templateRange; }
        }

        private ShiftArgs()
        {
        }
        public ShiftArgs(Size totalShift, Rectangle templateRange, ShiftDirection shiftDirection, bool reverseShift)
        {
            this.totalShift = totalShift;
            this.shiftDirection = shiftDirection;
            this.templateRange = templateRange;
            this.reverseShift = reverseShift;
        }
    }

    public interface IShiftEventSource
    {
        event EventHandler<EventArgs<ShiftArgs>> OnShift;
    }

    public class ShiftEventSource : IShiftEventSource
    {
        private readonly ITemplateEvaluator<string, object> _primitiveEvaluator;

        private readonly ClassValuedDictionary<string, object> collectionVars = new ClassValuedDictionary<string, object>();

        private object root;


        public ShiftEventSource(ITemplateEvaluator<string, object> templateEvaluator)
        {
            this._primitiveEvaluator = templateEvaluator;
        }


        public object Root
        {
            get { return this.root; }
            set { this.root = value; }
        }


        public Dictionary<string, object> CollectionVars
        {
            get { return this.collectionVars; }
        }

        private IDictionary<Tuple<ReportSectionInstance, TemplateSection>, Size> parallelsShiftMap;


        public IList<ReportInstance> Evaluate(ReportTemplate template, bool evaluateExceptionRaise)
        {
            this.parallelsShiftMap = new Dictionary<Tuple<ReportSectionInstance, TemplateSection>, Size>();
            template.Prepare(0);
            this.collectionVars[template.Name] = new object[] { this.root };
            var reportShift = new Size();
            return this.EvaluateSection(null, template, this.root, ref reportShift, evaluateExceptionRaise)
                  .ConvertAll(input => (ReportInstance)input);
        }

        private IEnumerable GetSectionCollection(TemplateSection templateSection, object rootObject, bool throwEvaluateException)
        {
            IEnumerable collection = null;
            if (rootObject == this.root)
            {
                collection = this.collectionVars[templateSection.Name] as IEnumerable;
            }
            if (null == collection)
            {
                collection = this._primitiveEvaluator.Evaluate(templateSection.Name, rootObject, this.collectionVars, throwEvaluateException) as IEnumerable;
            }
            if (collection is string) collection = null;
            if (null == collection)
            {
                collection = this._primitiveEvaluator.Evaluate("#" + templateSection.Name, rootObject, this.collectionVars, throwEvaluateException) as IEnumerable;
            }
            if (null == collection)
            {
                collection = this.collectionVars[templateSection.Name] as IEnumerable;
            }
            if (collection is string) collection = null;
            /*if (null == collection)
            {
                throw new EvaluationException("Collection " + templateSection.Name + " cannot be evaluated");
            }*/
            return collection ?? new List<object>();
        }

        private List<ReportSectionInstance> EvaluateSection(ReportSectionInstance parent, TemplateSection templateSection, object rootObject,
            ref Size currentShift, bool throwEvaluateException)
        {
            Size? prevParallelShift = null;
            if (null != templateSection.Parallel)
            {
                var shift = currentShift;
                this.parallelsShiftMap.Add(Tuple.Create(parent, templateSection), shift);
                var shift2 = currentShift;

                if (this.parallelsShiftMap.TryGetValue(Tuple.Create(parent, templateSection.Parallel), out shift2))
                {
                    prevParallelShift = shift;
                    currentShift = shift2;
                }
            }

            if (templateSection.ShiftDirection == ShiftDirection.TopDown)
            {
                currentShift.Width = 0;
            }
            IEnumerable collection = this.GetSectionCollection(templateSection, rootObject, throwEvaluateException);

            int count = 0;
            foreach (object obj in collection)
            {
                ReportSectionInstance result = templateSection.CreateReportSection(parent, count);
                templateSection.Instances.Add(result);
                if (count > 0)
                {
                    this.MakeShift(templateSection, ref currentShift, false);
                    if (null != parent)
                    {

                        Rectangle findRange = templateSection.Range;
                        findRange.Width = findRange.Width + 1;
                        findRange.Height = findRange.Height + 1;
                        foreach (ReportInstanceField reportInstanceField in parent.GetFieldsInTemplateRectangle(findRange))
                        {
                            result.Fields.Add(reportInstanceField.CloneAndShift(result, currentShift));
                        }
                    }
                }
                IList<TemplateFieldBase> affected = templateSection.TemplateFields;
                foreach (TemplateDetailsSection subSection in templateSection.SubSections)
                {
                    IList<TemplateFieldBase> unAffected;
                    ListUtils<TemplateFieldBase>.DivideCollection(
                            affected,
                            subSection.ShiftAffectsField,
                            out affected,
                            out unAffected);
                    foreach (TemplateFieldBase field in unAffected)
                    {
                        this.EvaluateTemplateField(result, field, obj, currentShift, throwEvaluateException);
                    }
                    this.EvaluateSection(result, subSection, obj, ref currentShift, throwEvaluateException);
                    //Copy fields from parent
                }
                if (null != affected)
                {
                    foreach (TemplateFieldBase field in affected)
                    {
                        this.EvaluateTemplateField(result, field, obj, currentShift, throwEvaluateException);
                    }
                }
                count++;
            }
            if (0 == count)
            {
                this.MakeShift(templateSection, ref currentShift, true);
            }
            if (null != prevParallelShift && currentShift.Height < prevParallelShift.Value.Height)
            {
                currentShift = prevParallelShift.Value;
            }
            return templateSection.Instances;
        }



        private ReportInstanceField EvaluateTemplateField(ReportSectionInstance parent, TemplateFieldBase field, object obj, Size currentShift, bool throwEvaluateException)
        {
            ReportInstanceField fieldInstance;
            if (field is TemplateField)
            {
                fieldInstance = new ReportInstanceField(parent, field, currentShift, this.GetCombinedValue((TemplateField)field, obj, throwEvaluateException));
            }
            else
            {
                fieldInstance = new ReportInstanceField(parent, field, currentShift, field.OriginalString);
            }
            parent.Fields.Add(fieldInstance);
            return fieldInstance;
        }
        public object GetCombinedValue(TemplateField field, object obj, bool throwEvaluateException)
        {
            if ((field.Expressions.Count == 1) && (field.Expressions[0].OriginalIndex == 0))
                return this._primitiveEvaluator.Evaluate(field.Expressions[0].Expression, obj, this.collectionVars, throwEvaluateException);
            string value = field.OriginalString;
            foreach (TemplateExpression expression in field.Expressions)
            {
                object valueObj = this._primitiveEvaluator.Evaluate(expression.Expression, obj, this.collectionVars, throwEvaluateException);
                string valueStr = (null == valueObj) ? string.Empty : valueObj.ToString();
                value = value.Replace(expression.OriginalString, valueStr);
            }
            return value;
        }
        private void MakeShift(TemplateSection templateSection, ref Size currentShift, bool reverse)
        {
            if (null != this.onShift)
            {
                ShiftArgs shiftArgs = new ShiftArgs(
                    currentShift,
                    templateSection.Range,
                    templateSection.ShiftDirection,
                    reverse);
                this.onShift(templateSection, new EventArgs<ShiftArgs>(shiftArgs));
                currentShift = shiftArgs.TotalShift;
            }

        }

        event EventHandler<EventArgs<ShiftArgs>> onShift;
        public event EventHandler<EventArgs<ShiftArgs>> OnShift
        {
            add { this.onShift += value; }
            remove { this.onShift -= value; }
        }
    }


    internal class ClassValuedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
        where TValue : class
    {
        public new TValue this[TKey key]
        {
            get
            {
                TValue value = null;
                this.TryGetValue(key, out value);
                return value;
            }
            set
            {
                base[key] = value;
            }
        }
        public ClassValuedDictionary()
            : base()
        {

        }
    }

    internal static class ListUtils<T>
    {
        public static List<TProp> GetPropertyValueList<TProp>(IEnumerable<T> list, Func<T, TProp> getProperty)
        {
            List<TProp> result = new List<TProp>();
            list.Foreach(delegate(T obj) { result.Add(getProperty(obj)); });
            return result;
        }

        public static void DivideCollection(IList<T> list, Predicate<T> predicate, out IList<T> trueList, out IList<T> falseList)
        {
            trueList = new List<T>();
            falseList = new List<T>();
            foreach (T obj in list)
            {
                if (predicate(obj))
                    trueList.Add(obj);
                else
                    falseList.Add(obj);
            }
        }
    }
}
