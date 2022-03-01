using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.EmployeeView)]
    public class TestRootSecurityObj : BaseDirectory, IMaster<TestSecurityObjItem>
    {
        private BusinessUnit businessUnit;

        private Location location;

        private readonly ICollection<TestSecurityObjItem> items = new List<TestSecurityObjItem>();

        private ManagementUnitFluentMapping managementUnitFluentMapping;

        public virtual IEnumerable<TestSecurityObjItem> Items => this.items;

        public virtual BusinessUnit BusinessUnit
        {
            get { return this.businessUnit; }
            set { this.businessUnit = value; }
        }
        
        public virtual ManagementUnitFluentMapping ManagementUnitFluentMapping
        {
            get { return this.managementUnitFluentMapping; }
            set { this.managementUnitFluentMapping = value; }
        }

        public virtual Location Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        ICollection<TestSecurityObjItem> IMaster<TestSecurityObjItem>.Details => (ICollection<TestSecurityObjItem>)this.Items;
    }
}
