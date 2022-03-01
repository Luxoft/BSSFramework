using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace WorkflowSampleSystem.Domain
{
    [View]
    public class ManagementUnitToAncestorChildView : AuditPersistentDomainObjectBase, IHierarchicalToAncestorOrChildLink<ManagementUnit, Guid>
    {
        private ManagementUnit childOrAncestor;
        private ManagementUnit source;

        public virtual ManagementUnit ChildOrAncestor
        {
            get { return this.childOrAncestor; }
        }

        public virtual ManagementUnit Source
        {
            get { return this.source; }
        }
    }
}
