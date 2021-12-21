using System;
using System.Collections.Generic;

namespace Framework.CustomReports.Domain
{
    public interface IAccessReportRight
    {
        IEnumerable<string> Principals { get; }

        IEnumerable<Guid> Operations { get; }

        IEnumerable<Guid> Roles { get; }
    }
}