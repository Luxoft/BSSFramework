using System;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public class ExcelHeaderDesign<T> : ExcelHeaderDesign
    {
        public ExcelHeaderDesign(string caption, ExcelDesignProperty<T> evaluateProperty)
            : this(caption)
        {
            this.DesignProperty = evaluateProperty;
        }

        public ExcelHeaderDesign(string caption) : base(caption)
        {
        }

        public ExcelDesignProperty<T> DesignProperty { get; private set; }
    }

    [Obsolete("Use ExcelHeaderDesign")]
    public class ExcelHeaderDesign
    {
        public static ExcelHeaderDesign<T> Create<T>(string caption, ExcelDesignProperty<T> evaluateProperty)
        {
            return new ExcelHeaderDesign<T>(caption, evaluateProperty);
        }

        public static ExcelHeaderDesign<T> Create<T>(string caption)
        {
            return new ExcelHeaderDesign<T>(caption);
        }

        protected ExcelHeaderDesign(string caption)
        {
            this.Caption = caption;
        }

        public string Caption
        {
            get;
            private set;
        }
    }
}
