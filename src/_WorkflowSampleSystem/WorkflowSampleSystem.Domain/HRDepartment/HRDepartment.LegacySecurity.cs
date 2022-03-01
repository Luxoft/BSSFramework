using System;

using Framework.Security;
using Framework.SecuritySystem;

namespace WorkflowSampleSystem.Domain
{
    [SecurityNode]
    public interface IDepartmentSecurityElement<out TDepartment>
    {
        TDepartment Department { get; }
    }

    [SecurityNode]
    public interface IDepartmentSecurityElement<out TDepartment, out TLocation> : IDepartmentSecurityElement<TDepartment>
        where TDepartment : ILocationSecurityElement<TLocation>, ISecurityContext
        where TLocation : ISecurityContext
    {
    }

    public partial class HRDepartment : ILocationSecurityElement<Location>
    {
    }
}
