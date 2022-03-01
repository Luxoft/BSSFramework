using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Security;

namespace WorkflowSampleSystem.Domain
{
    [DomainType("0BE31997-C4CD-449E-9394-A311016CB715")]
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.HRDepartmentView, WorkflowSampleSystemSecurityOperationCode.EmployeeEdit)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.HRDepartmentEdit)]
    public partial class HRDepartment :
        HRDepartmentBase,
        IDefaultHierarchicalPersistentDomainObjectBase<HRDepartment>,
        IMaster<HRDepartment>,
        IDetail<HRDepartment>
    {
        private readonly ICollection<HRDepartment> children = new List<HRDepartment>();

        private HRDepartment parent;

        public HRDepartment()
        {
        }

        public HRDepartment(HRDepartment parent)
            : this()
        {
            this.Parent = parent;
            parent?.children.Add(this);
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        [Framework.Restriction.Required]
        public override Location Location
        {
            get { return base.Location; }
            set { base.Location = value; }
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client | DTORole.Report)]
        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Event | DTORole.Integration)]
        public virtual IEnumerable<HRDepartment> Children
        {
            get { return this.children; }
        }

        public virtual HRDepartment Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        [CustomSerialization(CustomSerializationMode.Normal)]
        public override bool Active
        {
            get { return base.Active; }
            set { base.Active = value; }
        }

        ICollection<HRDepartment> IMaster<HRDepartment>.Details
        {
            get { return (ICollection<HRDepartment>)this.Children; }
        }

        HRDepartment IDetail<HRDepartment>.Master
        {
            get { return this.Parent; }
        }
    }
}
