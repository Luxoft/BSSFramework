using System;

namespace Framework.CustomReports.Attributes
{
    /// <summary>
    /// Attribute to control the display parameter name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ReportParameterDisplayNameAttribute : Attribute
    {
        public ReportParameterDisplayNameAttribute(string name)
        {
            this.Value = name;
        }

        public string Value { get; private set; }
    }
}
