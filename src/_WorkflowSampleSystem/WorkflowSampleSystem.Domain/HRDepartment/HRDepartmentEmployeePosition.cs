using Framework.Persistent;

namespace WorkflowSampleSystem.Domain
{
    public class HRDepartmentEmployeePosition : AuditPersistentDomainObjectBase, IDetail<HRDepartment>
    {
        private EmployeePosition employeePosition;
        private HRDepartment hrDepartment;

        public HRDepartmentEmployeePosition(HRDepartment hrDepartment)
        {
            this.hrDepartment = hrDepartment;
            hrDepartment.AddDetail(this);
        }

        public HRDepartmentEmployeePosition()
        {
        }

        [Framework.Restriction.UniqueElement]
        public virtual EmployeePosition EmployeePosition
        {
            get { return this.employeePosition; }
            set { this.employeePosition = value; }
        }

        [Framework.Restriction.UniqueElement]
        public virtual HRDepartment HrDepartment
        {
            get { return this.hrDepartment; }
            set { this.hrDepartment = value; }
        }

        HRDepartment IDetail<HRDepartment>.Master
        {
            get { return this.hrDepartment; }
        }
    }
}
