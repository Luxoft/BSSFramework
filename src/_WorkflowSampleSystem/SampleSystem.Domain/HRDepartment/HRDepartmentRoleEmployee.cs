using Framework.Persistent;

namespace SampleSystem.Domain
{
    public class HRDepartmentRoleEmployee : AuditPersistentDomainObjectBase, IDetail<HRDepartment>
    {
        private Employee employee;
        private HRDepartment hRDepartment;
        private HRDepartmentEmployeeRoleType hRDepartmentEmployeeRoleType;

        public HRDepartmentRoleEmployee(HRDepartment hRDepartment)
        {
            this.hRDepartment = hRDepartment;
            hRDepartment.AddDetail(this);
        }

        protected HRDepartmentRoleEmployee()
        {
        }

        [Framework.Restriction.Required]
        [Framework.Restriction.UniqueElement]
        public virtual Employee Employee
        {
            get { return this.employee; }
            set { this.employee = value; }
        }

        public virtual HRDepartment HRDepartment
        {
            get { return this.hRDepartment; }
        }

        [Framework.Restriction.UniqueElement]
        [Framework.Restriction.Required]
        public virtual HRDepartmentEmployeeRoleType HRDepartmentEmployeeRoleType
        {
            get { return this.hRDepartmentEmployeeRoleType; }
            set { this.hRDepartmentEmployeeRoleType = value; }
        }

        HRDepartment IDetail<HRDepartment>.Master
        {
            get { return this.hRDepartment; }
        }
    }
}
