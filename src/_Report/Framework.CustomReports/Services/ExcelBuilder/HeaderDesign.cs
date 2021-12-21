using System.Collections.Generic;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public class HeaderDesign<T>
    {
        public HeaderDesign(string caption, EvaluatePropertyInfo<T> evaluateProperty, double? width = null) : this(caption, new List<HeaderDesign<T>>(0), width)
        {
            this.Caption = caption;
            this.EvaluateProperty = evaluateProperty;
        }

        public HeaderDesign(string caption, IList<HeaderDesign<T>> subHeaders, double? width = null)
        {
            this.Caption = caption;
            this.SubHeaders = subHeaders;
            this.Width = width;
        }

        public string Caption { get; }

        public double? Width { get; }

        /// <summary>
        /// Стоит ли выполнять AutoFit для колонок на шите Data
        /// </summary>
        public bool AutoFit { get; set; } = false;

        public IList<HeaderDesign<T>> SubHeaders { get; }

        public EvaluatePropertyInfo<T> EvaluateProperty { get; }
    }

    public static class HeaderDesign
    {
        public static HeaderDesign<T> Create<T>(string caption, EvaluatePropertyInfo<T> evaluateProperty)
        {
            return new HeaderDesign<T>(caption, evaluateProperty);
        }

        public static HeaderDesign<T> Create<T>(string caption, IList<HeaderDesign<T>> subHeaders)
        {
            return new HeaderDesign<T>(caption, subHeaders);
        }
    }
}
