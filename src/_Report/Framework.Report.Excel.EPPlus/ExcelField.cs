using System;
using System.Drawing;

namespace Framework.Report.Excel.EPPlus
{
    public class ExcelField
    {
        private readonly FormatOptions formatOptions;
        private int column;
        private int row;
        private object value;
        private string worksheetName = string.Empty;

        public ExcelField(ExcelField field)
        {
            this.row = field.row;
            this.column = field.column;
            this.value = field.value;
            this.worksheetName = field.worksheetName;
        }

        public ExcelField(int row, int column, object value)
        {
            this.row = row;
            this.column = column;
            this.value = value;
        }

        public ExcelField(int row, int column, object value, FormatOptions options)
            : this(row, column, value)
        {
            this.formatOptions = options;
        }

        protected ExcelField()
        {
        }

        public virtual string WorksheetName
        {
            get { return this.worksheetName; }
            set { this.worksheetName = value; }
        }

        public virtual int Row
        {
            get { return this.row; }
            set { this.row = value; }
        }

        public virtual int Column
        {
            get { return this.column; }
            set { this.column = value; }
        }

        public virtual object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public virtual FormatOptions Options
        {
            get { return this.formatOptions; }
        }

        public static string GetFormulaArg(int row, int col)
        {
            if (0 >= col)
            {
                throw new ArgumentException("col must be greater than 0");
            }

            if (0 >= row)
            {
                throw new ArgumentException("row must be greater than 0");
            }

            return $"{GetColumnLetterByIndex(col)}{row}";
        }

        /// <summary>
        /// Return Column char name (ex: A,B,C,...,AA,AB...) from index
        /// </summary>
        public static string GetColumnLetterByIndex(int index)
        {
            string result = string.Empty;
            int alpha = index / 27;
            int remainder = index - (alpha * 26);

            if (alpha > 0)
            {
                result = char.ConvertFromUtf32(alpha + 64);
            }

            if (remainder > 0)
            {
                result = result + char.ConvertFromUtf32(remainder + 64);
            }

            return result;
        }

        #region Nested type: FormatOptions

        public class FormatOptions
        {
            private Color backgroundColor = Color.Transparent;
            private bool bold;
            private int colorIndex;
            private int fontSize = 12;

            public FormatOptions(bool bold, int fontSize, Color backgroundColor)
            {
                this.bold = bold;
                this.fontSize = fontSize;
                this.backgroundColor = backgroundColor;
            }

            public FormatOptions(bool bold, int fontSize, int colorIndex)
            {
                this.bold = bold;
                this.fontSize = fontSize;
                this.colorIndex = colorIndex;
            }

            public bool Bold
            {
                get { return this.bold; }
                set { this.bold = value; }
            }

            public int FontSize
            {
                get { return this.fontSize; }
                set { this.fontSize = value; }
            }

            public Color BackgroundColor
            {
                get { return this.backgroundColor; }
                set { this.backgroundColor = value; }
            }

            public int ColorIndex
            {
                get { return this.colorIndex; }
                set { this.colorIndex = value; }
            }
        }

        #endregion
    }
}