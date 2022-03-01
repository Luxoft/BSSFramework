using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole, BLLRemoveRole, BLLSaveRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.ManagementUnitAndBusinessUnitLinkView)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.ManagementUnitAndBusinessUnitLinkEdit)]
    public class ManagementUnitAndBusinessUnitLink :
        AuditPersistentDomainObjectBase,
        IDetail<ManagementUnit>,
        IDetail<BusinessUnit>,
        IVisualIdentityObject
    {
        private BusinessUnit businessUnit;
        private ManagementUnit managementUnit;
        private bool equalBU;

        public ManagementUnitAndBusinessUnitLink(ManagementUnit managementUnit)
        {
            this.managementUnit = managementUnit;
            this.managementUnit.Maybe(z => z.AddDetail(this));
        }

        public ManagementUnitAndBusinessUnitLink(ManagementUnit managementUnit, BusinessUnit businessUnit)
            : this(managementUnit)
        {
            this.businessUnit = businessUnit;
        }

        public ManagementUnitAndBusinessUnitLink(BusinessUnit businessUnit)
        {
            this.businessUnit = businessUnit;
            this.businessUnit.Maybe(z => z.AddDetail(this));
        }

        public ManagementUnitAndBusinessUnitLink(BusinessUnit businessUnit, ManagementUnit managementUnit)
            : this(businessUnit)
        {
            this.managementUnit = managementUnit;
        }

        public ManagementUnitAndBusinessUnitLink()
        {
        }

        public virtual bool EqualBU
        {
            get { return this.equalBU; }
            set { this.equalBU = value; }
        }

        [Framework.Restriction.Required]
        [Framework.Restriction.UniqueElement]
        public virtual ManagementUnit ManagementUnit
        {
            get { return this.managementUnit; }
            set { this.managementUnit = value; }
        }

        [Framework.Restriction.Required]
        [Framework.Restriction.UniqueElement]
        public virtual BusinessUnit BusinessUnit
        {
            get { return this.businessUnit; }
            set { this.businessUnit = value; }
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Event | DTORole.Integration)]
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Client)]
        [ExpandPath("ManagementUnit.HRDepartments")]
        [DetailRole(false)]
        public virtual IEnumerable<HRDepartment> LinkedHRDepartments
        {
            get { return this.ManagementUnit.HRDepartments.ToList(link => link.HRDepartment); }
        }

        ManagementUnit IDetail<ManagementUnit>.Master
        {
            get
            {
                return this.managementUnit;
            }
        }

        BusinessUnit IDetail<BusinessUnit>.Master
        {
            get
            {
                return this.businessUnit;
            }
        }

        string IVisualIdentityObject.Name
        {
            get { return this.BusinessUnit.Maybe(x => x.Name) + "-" + this.ManagementUnit.Maybe(x => x.Name); }
        }
    }
}
