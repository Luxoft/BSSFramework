using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Security;
using Framework.SecuritySystem;

namespace SampleSystem.Domain
{
    [DomainType("CACA9DB4-9DA6-48AA-9FD3-A311016CB715")]
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.LocationView, SampleSystemSecurityOperationCode.HRDepartmentEdit)]
    [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.LocationEdit)]
    [UniqueGroup]
    public partial class Location :
        BaseDirectory,
        IDefaultHierarchicalPersistentDomainObjectBase<Location>,
        IMaster<Location>,
        IDetail<Location>,
        ISecurityContext
    {
        private readonly ICollection<Location> children = new List<Location>();

        private Country country;
        private bool isFinancial;
        private LocationType locationType;
        private Location parent;

        private int closeDate;
        private int code;

        private byte[] binaryData;

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

        public virtual byte[] BinaryData
        {
            get => this.binaryData;
            set => this.binaryData = value;
        }

        [FetchPath("Children")]
        public virtual bool IsLeaf
        {
            get { return !this.Children.Any(); }
        }

        [FetchPath("Children")]
        public virtual bool ContainsOnlyInactiveChildren
        {
            get { return this.Children.All(x => !x.Active); }
        }

        public virtual Country Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual Location Root
        {
            get { return this.Parent == null ? this : this.Parent.Root; }
        }

        public virtual LocationType LocationType
        {
            get { return this.locationType; }
            set { this.locationType = value; }
        }

        public virtual bool IsFinancial
        {
            get { return this.isFinancial; }
            set { this.isFinancial = value; }
        }

        [Required]
        public virtual int CloseDate
        {
            get { return this.closeDate; }
            set { this.closeDate = value; }
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

        [Required]
        public virtual int Code
        {
            get
            {
                return this.code;
            }

            set
            {
                this.code = value;
            }
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
