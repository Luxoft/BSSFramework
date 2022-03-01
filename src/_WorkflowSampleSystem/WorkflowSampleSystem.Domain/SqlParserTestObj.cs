using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Transfering;

namespace WorkflowSampleSystem.Domain
{
    [DomainType("{4963D86E-5650-41E0-BDBA-0274FF2CF375}")]
    [BLLViewRole(Max = MainDTOType.FullDTO)]
    [BLLSaveRole]
    [BLLRemoveRole]
    [UniqueGroup]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    public class SqlParserTestObj : AuditPersistentDomainObjectBase
    {
        private string notNullColumn;

        private string uniqueColumn;

        public virtual string NotNullColumn
        {
            get { return this.notNullColumn; }
            set { this.notNullColumn = value; }
        }

        [UniqueElement]
        public virtual string UniqueColumn
        {
            get { return this.uniqueColumn; }
            set { this.uniqueColumn = value; }
        }
    }
}
