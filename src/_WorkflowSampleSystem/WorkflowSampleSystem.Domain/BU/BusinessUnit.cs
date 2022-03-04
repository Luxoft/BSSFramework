using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace WorkflowSampleSystem.Domain
{
    [DomainType("5C326B10-B4B4-402C-BCCE-A311016CB715")]
    [BLLViewRole, BLLSaveRole(AllowCreate = false)]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.BusinessUnitView, SourceTypes = new[] { typeof(Employee) })]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.BusinessUnitEdit)]
    public partial class BusinessUnit : BaseDirectory,
                                        IDenormalizedHierarchicalPersistentSource<BusinessUnitAncestorLink, BusinessUnitToAncestorChildView, BusinessUnit, Guid>,
                                        IDefaultHierarchicalPersistentDomainObjectBase<BusinessUnit>,
                                        IMaster<BusinessUnit>,
                                        IDetail<BusinessUnit>,
                                        ISecurityContext
    {
        private readonly ICollection<BusinessUnit> children = new List<BusinessUnit>();

        private BusinessUnit parent;

        private Period period;

        public BusinessUnit()
        {
        }

        public BusinessUnit(BusinessUnit parent)
        {
            this.Parent = parent;
            if (null != parent)
            {
                parent.AddDetail(this);
            }
        }

        public virtual Period Period
        {
            get { return this.period; }
            protected internal set { this.period = value; }
        }

        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.Integration | DTORole.Event)]
        [CustomSerialization(CustomSerializationMode.ReadOnly, DTORole.Client)]
        public virtual IEnumerable<BusinessUnit> Children
        {
            get { return this.children; }
        }

        /// <summary>
        /// Supposed to be set from dto only.
        /// </summary>
        [IsMaster]
        public virtual BusinessUnit Parent
        {
            get { return this.parent; }
            protected internal set { this.parent = value; }
        }

        ICollection<BusinessUnit> IMaster<BusinessUnit>.Details
        {
            get { return (ICollection<BusinessUnit>)this.Children; }
        }

        BusinessUnit IDetail<BusinessUnit>.Master
        {
            get { return this.Parent; }
        }
    }
}
