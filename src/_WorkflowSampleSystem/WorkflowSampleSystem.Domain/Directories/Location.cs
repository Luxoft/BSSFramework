using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;
using Framework.SecuritySystem;

namespace WorkflowSampleSystem.Domain
{
    [DomainType("CACA9DB4-9DA6-48AA-9FD3-A311016CB715")]
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.LocationView, WorkflowSampleSystemSecurityOperationCode.HRDepartmentEdit)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.LocationEdit)]
    [UniqueGroup]
    public class Location :
        BaseDirectory,
        IDefaultHierarchicalPersistentDomainObjectBase<Location>,
        IMaster<Location>,
        IDetail<Location>,
        ISecurityContext
    {
        private readonly ICollection<Location> children = new List<Location>();

        private Location parent;

        public Location()
        {
        }

        public Location(Location parent)
        {
            if (parent != null)
            {
                this.parent = parent;
                this.parent.AddDetail(this);
            }
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual IEnumerable<Location> Children
        {
            get { return this.children; }
        }

        public virtual Location Parent
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

        Location IDetail<Location>.Master
        {
            get { return this.Parent; }
        }

        ICollection<Location> IMaster<Location>.Details
        {
            get { return (ICollection<Location>)this.Children; }
        }
    }
}
