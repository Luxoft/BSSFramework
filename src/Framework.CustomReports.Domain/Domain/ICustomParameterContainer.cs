using System;

namespace Framework.CustomReports.Domain
{
    public interface ICustomParameterContainer
    {
        Type ParameterType { get; }
    }
}