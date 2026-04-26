using Framework.BLL.Domain.Attributes;

using SampleSystem.Domain.Directories;

using Anch.SecuritySystem;

namespace SampleSystem.Domain.HRDepartment;

[SecurityNode]
public interface IDepartmentSecurityElement<out TDepartment>
{
    TDepartment Department { get; }
}

[SecurityNode]
public interface IDepartmentSecurityElement<out TDepartment, out TLocation> : IDepartmentSecurityElement<TDepartment>
        where TDepartment : ILocationSecurityElement<TLocation>, ISecurityContext
        where TLocation : ISecurityContext;

public partial class HRDepartment : ILocationSecurityElement<Location>;
