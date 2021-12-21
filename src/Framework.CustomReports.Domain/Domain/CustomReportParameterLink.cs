using System;

namespace Framework.CustomReports.Domain
{
    public class CustomReportParameterLink
    {
        public CustomReportParameterLink(Type customReportType, Type parameterType)
        {
            this.CustomReportType = customReportType;
            this.ParameterType = parameterType;
        }

        public Type CustomReportType{get; }

        public Type ParameterType { get; }
    }
}