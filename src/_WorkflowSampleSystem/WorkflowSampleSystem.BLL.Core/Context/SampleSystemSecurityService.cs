using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL
{
    public partial class WorkflowSampleSystemSecurityService
    {
        public override WorkflowSampleSystemSecurityPath<TDomainObject> GetEmployeeSecurityPath<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>()
        {
            return WorkflowSampleSystemSecurityPath<TDomainObject>.Create(v => v.Employee).And(v => v.BusinessUnit).And(v => v.Department.Location);
        }

        public override WorkflowSampleSystemSecurityPath<BusinessUnit> GetBusinessUnitSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<BusinessUnit>.Create(v => v);
        }

        public override WorkflowSampleSystemSecurityPath<EmployeeCellPhone> GetEmployeeCellPhoneSecurityPath()
        {
            return this.GetEmployeeSecurityPath<Employee, BusinessUnit, HRDepartment, Location, Employee>().OverrideInput<EmployeeCellPhone>(cellPhone => cellPhone.Employee);
        }

        public override WorkflowSampleSystemSecurityPath<BusinessUnitHrDepartment> GetBusinessUnitHrDepartmentSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<BusinessUnitHrDepartment>.Create(v => v.BusinessUnit).And(v => v.HRDepartment.Location);
        }

        public override WorkflowSampleSystemSecurityPath<BusinessUnitManagerCommissionLink> GetBusinessUnitManagerCommissionLinkSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<BusinessUnitManagerCommissionLink>.Create(v => v.BusinessUnit);
        }

        public override WorkflowSampleSystemSecurityPath<EmployeePhoto> GetEmployeePhotoSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<EmployeePhoto>.Create(employeePhoto => employeePhoto.Employee.CoreBusinessUnit);
        }

        public override WorkflowSampleSystemSecurityPath<EmployeePosition> GetEmployeePositionSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<EmployeePosition>.Create(position => position.Location);
        }

        public override WorkflowSampleSystemSecurityPath<ManagementUnitFluentMapping> GetManagementUnitFluentMappingSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<ManagementUnitFluentMapping>.Create(v => v);
        }

        public override WorkflowSampleSystemSecurityPath<ManagementUnit> GetManagementUnitSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<ManagementUnit>.Create(v => v);
        }

        public override WorkflowSampleSystemSecurityPath<TestRootSecurityObj> GetTestRootSecurityObjSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<TestRootSecurityObj>.Create(v => v.BusinessUnit).And(v => v.Location);
        }

        public override WorkflowSampleSystemSecurityPath<ManagementUnitAndBusinessUnitLink> GetManagementUnitAndBusinessUnitLinkSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<ManagementUnitAndBusinessUnitLink>.Create(v => v.BusinessUnit).And(v => v.ManagementUnit);
        }

        public override WorkflowSampleSystemSecurityPath<ManagementUnitAndHRDepartmentLink> GetManagementUnitAndHRDepartmentLinkSecurityPath()
        {
            return WorkflowSampleSystemSecurityPath<ManagementUnitAndHRDepartmentLink>.Create(v => v.ManagementUnit).And(v => v.HRDepartment.Location);
        }
    }
}
