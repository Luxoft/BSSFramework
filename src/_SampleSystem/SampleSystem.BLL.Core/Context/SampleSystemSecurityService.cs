using System.Linq;

using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial class SampleSystemSecurityService
{
    public override SampleSystemSecurityPath<TDomainObject> GetEmployeeSecurityPath<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>()
    {
        return SampleSystemSecurityPath<TDomainObject>.Create(v => v.Employee).And(v => v.BusinessUnit).And(v => v.Department.Location);
    }

    public override SampleSystemSecurityPath<BusinessUnit> GetBusinessUnitSecurityPath()
    {
        return SampleSystemSecurityPath<BusinessUnit>.Create(v => v);
    }

    public override SampleSystemSecurityPath<EmployeeCellPhone> GetEmployeeCellPhoneSecurityPath()
    {
        return this.GetEmployeeSecurityPath<Employee, BusinessUnit, HRDepartment, Location, Employee>().OverrideInput<EmployeeCellPhone>(cellPhone => cellPhone.Employee);
    }

    public override SampleSystemSecurityPath<BusinessUnitHrDepartment> GetBusinessUnitHrDepartmentSecurityPath()
    {
        return SampleSystemSecurityPath<BusinessUnitHrDepartment>.Create(v => v.BusinessUnit).And(v => v.HRDepartment.Location);
    }

    public override SampleSystemSecurityPath<BusinessUnitManagerCommissionLink> GetBusinessUnitManagerCommissionLinkSecurityPath()
    {
        return SampleSystemSecurityPath<BusinessUnitManagerCommissionLink>.Create(v => v.BusinessUnit);
    }

    public override SampleSystemSecurityPath<EmployeePhoto> GetEmployeePhotoSecurityPath()
    {
        return SampleSystemSecurityPath<EmployeePhoto>.Create(employeePhoto => employeePhoto.Employee.CoreBusinessUnit);
    }

    public override SampleSystemSecurityPath<EmployeePosition> GetEmployeePositionSecurityPath()
    {
        return SampleSystemSecurityPath<EmployeePosition>.Create(position => position.Location);
    }

    public override SampleSystemSecurityPath<ManagementUnitFluentMapping> GetManagementUnitFluentMappingSecurityPath()
    {
        return SampleSystemSecurityPath<ManagementUnitFluentMapping>.Create(v => v);
    }

    public override SampleSystemSecurityPath<ManagementUnit> GetManagementUnitSecurityPath()
    {
        return SampleSystemSecurityPath<ManagementUnit>.Create(v => v);
    }

    public override SampleSystemSecurityPath<TestPerformanceObject> GetTestPerformanceObjectSecurityPath()
    {
        return SampleSystemSecurityPath<TestPerformanceObject>.Create(v => v.Location, SingleSecurityMode.Strictly)
                                                              .And(v => v.Employee, SingleSecurityMode.Strictly)
                                                              .And(v => v.BusinessUnit, SingleSecurityMode.Strictly)
                                                              .And(v => v.ManagementUnit, SingleSecurityMode.Strictly);
    }


    public override SampleSystemSecurityPath<TestPlainAuthObject> GetTestPlainAuthObjectSecurityPath()
    {
        return SampleSystemSecurityPath<TestPlainAuthObject>.Create(v => v.Location)
                                                            .And(v => v.Items.Select(item => item.BusinessUnit), ManySecurityPathMode.All)
                                                            .And(v => v.Items.Select(item => item.ManagementUnit), ManySecurityPathMode.All);
    }

    public override SampleSystemSecurityPath<TestRootSecurityObj> GetTestRootSecurityObjSecurityPath()
    {
        return SampleSystemSecurityPath<TestRootSecurityObj>.Create(v => v.BusinessUnit).And(v => v.Location);
    }

    public override SampleSystemSecurityPath<ManagementUnitAndBusinessUnitLink> GetManagementUnitAndBusinessUnitLinkSecurityPath()
    {
        return SampleSystemSecurityPath<ManagementUnitAndBusinessUnitLink>.Create(v => v.BusinessUnit).And(v => v.ManagementUnit);
    }

    public override SampleSystemSecurityPath<ManagementUnitAndHRDepartmentLink> GetManagementUnitAndHRDepartmentLinkSecurityPath()
    {
        return SampleSystemSecurityPath<ManagementUnitAndHRDepartmentLink>.Create(v => v.ManagementUnit).And(v => v.HRDepartment.Location);
    }
}
