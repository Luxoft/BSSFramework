using System;
using System.Collections.Generic;

namespace Framework.CustomReports.Domain
{
    public interface ICustomReport<TSecurityOperationCode> : ICustomParameterContainer
    {
        Guid Id { get; }

        string Name { get; }

        string Description { get; }

        IAccessReportRight AccessReportRight { get; }

        Type ParameterType { get; }

        TSecurityOperationCode SecurityOperation { get; }
    }
}