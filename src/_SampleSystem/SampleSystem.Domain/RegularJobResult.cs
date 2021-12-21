using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain
{
    [BLLViewRole]
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.Disabled)]
    public class RegularJobResult : AuditPersistentDomainObjectBase
    {
        private string testValue;

        public virtual string TestValue
        {
            get => this.testValue;
            set => this.testValue = value;
        }
    }
}
