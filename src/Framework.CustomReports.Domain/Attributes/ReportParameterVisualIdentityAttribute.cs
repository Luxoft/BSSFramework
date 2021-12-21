using System;

namespace Framework.CustomReports.Attributes
{
    /// <summary>
    /// Attribute to control the display parameter value name
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ReportParameterVisualIdentityAttribute : Attribute
    {
        public ReportParameterVisualIdentityAttribute(string name)
        {
            this.Value = name;
        }

        public string Value { get; private set; }
    }
}
