using System;

using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace WorkflowSampleSystem.Domain
{
    [View]
    public class BusinessUnitToAncestorChildView : AuditPersistentDomainObjectBase, IHierarchicalToAncestorOrChildLink<BusinessUnit, Guid>
    {
        private BusinessUnit childOrAncestor;

        private BusinessUnit source;


        public virtual BusinessUnit ChildOrAncestor => this.childOrAncestor;

        public virtual BusinessUnit Source => this.source;
    }
}