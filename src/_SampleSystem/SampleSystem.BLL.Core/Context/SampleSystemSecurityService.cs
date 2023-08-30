using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial class SampleSystemSecurityService
{
    public override SecurityPath<TDomainObject> GetEmployeeSecurityPath<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>()
    {
        return SecurityPath<TDomainObject>.Create(v => v.Employee).And(v => v.BusinessUnit).And(v => v.Department.Location);
    }

    public override SecurityPath<BusinessUnit> GetBusinessUnitSecurityPath()
    {
        return SecurityPath<BusinessUnit>.Create(v => v);
    }

    public override SecurityPath<EmployeeCellPhone> GetEmployeeCellPhoneSecurityPath()
    {
        return this.GetEmployeeSecurityPath<Employee, BusinessUnit, HRDepartment, Location, Employee>().OverrideInput<EmployeeCellPhone>(cellPhone => cellPhone.Employee);
    }

    public override SecurityPath<AuthPerformanceObject> GetAuthPerformanceObjectSecurityPath()
    {
        return SecurityPath<AuthPerformanceObject>.Create(v => v.BusinessUnit)
                                                              .And(v => v.ManagementUnit)
                                                              .And(v => v.Location)
                                                              .And(v => v.Employee);
    }

    public override SecurityPath<BusinessUnitHrDepartment> GetBusinessUnitHrDepartmentSecurityPath()
    {
        return SecurityPath<BusinessUnitHrDepartment>.Create(v => v.BusinessUnit).And(v => v.HRDepartment.Location);
    }

    public override SecurityPath<BusinessUnitManagerCommissionLink> GetBusinessUnitManagerCommissionLinkSecurityPath()
    {
        return SecurityPath<BusinessUnitManagerCommissionLink>.Create(v => v.BusinessUnit);
    }

    public override SecurityPath<EmployeePhoto> GetEmployeePhotoSecurityPath()
    {
        return SecurityPath<EmployeePhoto>.Create(employeePhoto => employeePhoto.Employee.CoreBusinessUnit);
    }

    public override SecurityPath<EmployeePosition> GetEmployeePositionSecurityPath()
    {
        return SecurityPath<EmployeePosition>.Create(position => position.Location);
    }

    public override SecurityPath<ManagementUnitFluentMapping> GetManagementUnitFluentMappingSecurityPath()
    {
        return SecurityPath<ManagementUnitFluentMapping>.Create(v => v);
    }

    public override SecurityPath<ManagementUnit> GetManagementUnitSecurityPath()
    {
        return SecurityPath<ManagementUnit>.Create(v => v);
    }

    public override SecurityPath<TestPerformanceObject> GetTestPerformanceObjectSecurityPath()
    {
        return SecurityPath<TestPerformanceObject>.Create(v => v.Location, SingleSecurityMode.Strictly)
                                                              .And(v => v.Employee, SingleSecurityMode.Strictly)
                                                              .And(v => v.BusinessUnit, SingleSecurityMode.Strictly)
                                                              .And(v => v.ManagementUnit, SingleSecurityMode.Strictly);
    }


    public override SecurityPath<TestPlainAuthObject> GetTestPlainAuthObjectSecurityPath()
    {
        return SecurityPath<TestPlainAuthObject>.Create(v => v.Location)
                                                            .And(v => v.Items.Select(item => item.BusinessUnit), ManySecurityPathMode.All)
                                                            .And(v => v.Items.Select(item => item.ManagementUnit), ManySecurityPathMode.All);
    }

    public override SecurityPath<TestRootSecurityObj> GetTestRootSecurityObjSecurityPath()
    {
        return SecurityPath<TestRootSecurityObj>.Create(v => v.BusinessUnit).And(v => v.Location);
    }

    public override SecurityPath<ManagementUnitAndBusinessUnitLink> GetManagementUnitAndBusinessUnitLinkSecurityPath()
    {
        return SecurityPath<ManagementUnitAndBusinessUnitLink>.Create(v => v.BusinessUnit).And(v => v.ManagementUnit);
    }

    public override SecurityPath<ManagementUnitAndHRDepartmentLink> GetManagementUnitAndHRDepartmentLinkSecurityPath()
    {
        return SecurityPath<ManagementUnitAndHRDepartmentLink>.Create(v => v.ManagementUnit).And(v => v.HRDepartment.Location);
    }
}
