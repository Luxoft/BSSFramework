using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;
using Framework.Transfering;

namespace WorkflowSampleSystem.Domain
{
    [DomainType("{6502514C-2B88-40BF-8D01-C3DFAB008599}")]
    [BLLViewRole(Max = MainDTOType.FullDTO)]
    [BLLSaveRole]
    [UniqueGroup]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    public class SqlParserTestObjContainer : AuditPersistentDomainObjectBase
    {
        private SqlParserTestObj includedObject;

        public virtual SqlParserTestObj IncludedObject
        {
            get { return this.includedObject; }
            set { this.includedObject = value; }
        }
    }
}
