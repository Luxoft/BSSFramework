namespace Framework.CustomReports.Services
{
    internal struct EvaluatedFilter
    {
        public EvaluatedFilter(string[] propertyChain, string filterOperator, string value) : this()
        {
            this.PropertyChain = propertyChain;
            this.FilterOperator = filterOperator;
            this.Value = value;
        }

        public string[] PropertyChain { get; }

        public string FilterOperator { get; }

        public string Value { get; }
    }
}