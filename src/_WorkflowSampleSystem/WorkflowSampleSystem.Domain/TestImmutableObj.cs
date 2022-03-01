using Framework.DomainDriven.BLL;

namespace WorkflowSampleSystem.Domain
{
    [BLLViewRole]
    [BLLSaveRole]
    [BLLIntegrationSaveRole]
    [WorkflowSampleSystemViewDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    [WorkflowSampleSystemEditDomainObject(WorkflowSampleSystemSecurityOperationCode.Disabled)]
    public class TestImmutableObj : AuditPersistentDomainObjectBase
    {
        private string testImmutablePrimitiveProperty;

        private Employee testImmutableRefProperty;

        [FixedPropertyValidator]
        public virtual string TestImmutablePrimitiveProperty
        {
            get => this.testImmutablePrimitiveProperty;
            set => this.testImmutablePrimitiveProperty = value;
        }

        [FixedPropertyValidator]
        public virtual Employee TestImmutableRefProperty
        {
            get => this.testImmutableRefProperty;
            set => this.testImmutableRefProperty = value;
        }
    }
}
