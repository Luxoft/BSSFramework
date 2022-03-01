using System;

using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

using JetBrains.Annotations;

namespace WorkflowSampleSystem.Domain
{
    public class BusinessUnitEmployeeRole : AuditPersistentDomainObjectBase, IDetail<BusinessUnit>
    {
        private BusinessUnit businessUnit;
        private Employee employee;
        private BusinessUnitEmployeeRoleType role;

        public BusinessUnitEmployeeRole([NotNull] BusinessUnit businessUnit)
        {
            if (businessUnit == null) throw new ArgumentNullException(nameof(businessUnit));

            this.businessUnit = businessUnit;
            this.businessUnit.Maybe(z => z.AddDetail(this));
        }

        protected BusinessUnitEmployeeRole()
        {
        }

        [Required]
        public virtual BusinessUnit BusinessUnit
        {
            get { return this.businessUnit; }
        }

        [Required]
        public virtual Employee Employee
        {
            get { return this.employee; }
            set { this.employee = value; }
        }

        public virtual BusinessUnitEmployeeRoleType Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        BusinessUnit IDetail<BusinessUnit>.Master
        {
            get { return this.businessUnit; }
        }

        public static BusinessUnitEmployeeRole Create(
            [NotNull] BusinessUnit businessUnit,
            [NotNull] Employee employee,
            BusinessUnitEmployeeRoleType role)
        {
            if (businessUnit == null)
            {
                throw new ArgumentNullException(nameof(businessUnit));
            }

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            return new BusinessUnitEmployeeRole(businessUnit)
            {
                Employee = employee,
                Role = role
            };
        }
    }
}
