namespace Framework.Configuration.Domain.Models.Custom.Reports
{
    public class ReportGenerationPredefineValue
    {
        public ReportGenerationPredefineValue(string name, string designValue)
        {
            this.Name = name;
            this.DesignValue = designValue;
        }

        public string Name { get; set; }

        public string DesignValue { get; set; }
    }
}