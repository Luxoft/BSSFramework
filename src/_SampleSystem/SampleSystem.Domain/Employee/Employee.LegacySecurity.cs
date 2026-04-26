using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Persistent.Attributes;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.HRDepartment;

using Anch.SecuritySystem;

namespace SampleSystem.Domain.Employee;

[SecurityNode]
public interface IEmployeeSecurityElement<out TEmployee>
        where TEmployee : ISecurityContext
{
    TEmployee Employee { get; }
}

[SecurityNode]
public interface IEmployeeSecurityElement<out TEmployee, out TBusinessUnit, out TDepartment, out TLocation> : IEmployeeSecurityElement<TEmployee>
        where TEmployee : IEmployeeSecurity<TBusinessUnit, TDepartment, TLocation>, ISecurityContext
        where TBusinessUnit : ISecurityContext
        where TDepartment : ILocationSecurityElement<TLocation>
        where TLocation : ISecurityContext;

[SecurityNode]
public interface IEmployeeSecurity<out TBusinessUnit, out TDepartment, out TLocation> : IBusinessUnitSecurityElement<TBusinessUnit>, IDepartmentSecurityElement<TDepartment>
        where TBusinessUnit : ISecurityContext
        where TDepartment : ILocationSecurityElement<TLocation>
        where TLocation : ISecurityContext
{
    string Login { get; }
}

public partial class Employee : IEmployeeSecurity<BusinessUnit, HRDepartment.HRDepartment, Location>, IEmployeeSecurityElement<Employee, BusinessUnit, HRDepartment.HRDepartment, Location>
{
    [ExpandPath("")]
    Employee IEmployeeSecurityElement<Employee>.Employee => this;

    [ExpandPath(nameof(CoreBusinessUnit))]
    BusinessUnit IBusinessUnitSecurityElement<BusinessUnit>.BusinessUnit => this.CoreBusinessUnit;

    [ExpandPath(nameof(HRDepartment))]
    HRDepartment.HRDepartment IDepartmentSecurityElement<HRDepartment.HRDepartment>.Department => this.HRDepartment;
}
