namespace Framework.CustomReports.Services.ExcelBuilder
{
    public struct EvaluateParameterInfoItem
    {
        public EvaluateParameterInfoItem(string name, string value) : this()
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; private set; }
        public string Value { get; private set; }
    }
}