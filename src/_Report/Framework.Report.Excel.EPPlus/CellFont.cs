namespace Framework.Report.Excel.EPPlus
{
    public struct CellFont
    {
        public bool IsBold;
        public bool IsItalic;
        public double Size;
        public string Name;
        public string Color;

        public static bool operator ==(CellFont fontOne, CellFont fontTwo)
        {
            return fontOne.Equals(fontTwo);
        }

        public static bool operator !=(CellFont fontOne, CellFont fontTwo)
        {
            return !fontOne.Equals(fontTwo);
        }
    }
}
