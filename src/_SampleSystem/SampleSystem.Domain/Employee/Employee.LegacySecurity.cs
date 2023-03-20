using System;

using Framework.Persistent;
using Framework.Security;
using Framework.SecuritySystem;

namespace SampleSystem.Domain;

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
        where TLocation : ISecurityContext
{
}

[SecurityNode]
public interface IEmployeeSecurity<out TBusinessUnit, out TDepartment, out TLocation> : IBusinessUnitSecurityElement<TBusinessUnit>, IDepartmentSecurityElement<TDepartment>
        where TBusinessUnit : ISecurityContext
        where TDepartment : ILocationSecurityElement<TLocation>
        where TLocation : ISecurityContext
{
    string Login { get; }
}

public partial class Employee : IEmployeeSecurity<BusinessUnit, HRDepartment, Location>, IEmployeeSecurityElement<Employee, BusinessUnit, HRDepartment, Location>
{
    [ExpandPath("")]
    Employee IEmployeeSecurityElement<Employee>.Employee => this;

    [ExpandPath(nameof(CoreBusinessUnit))]
    BusinessUnit IBusinessUnitSecurityElement<BusinessUnit>.BusinessUnit => this.CoreBusinessUnit;

    [ExpandPath(nameof(HRDepartment))]
    HRDepartment IDepartmentSecurityElement<HRDepartment>.Department => this.HRDepartment;
}
