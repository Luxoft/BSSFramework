using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace SampleSystem.Domain
{
    [DomainType("D7F52AD2-70B9-49DB-9AD1-E85D5A869A33")]
    [Table(Name = "MessageTemplate", Schema = "configuration")]
    [BLLViewRole]
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.Disabled)]
    public class SampleSystemMessageTemplate : AuditPersistentDomainObjectBase
    {
        private string code;

        public virtual string Code
        {
            get => this.code;
            set => this.code = value;
        }
    }
}
