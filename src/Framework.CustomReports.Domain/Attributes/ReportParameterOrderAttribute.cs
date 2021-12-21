using System;

namespace Framework.CustomReports.Attributes
{
    /// <summary>
    /// Attribute to control the order of display parameters
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ReportParameterOrderAttribute : Attribute
    {
        public ReportParameterOrderAttribute(int value)
        {
            this.Value = value;
        }

        public int Value { get; private set; }
    }
}
