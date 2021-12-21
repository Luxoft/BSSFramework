using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain
{
    [DomainType("11CADB71-8434-4C9D-ABEE-456C8CACE7E3")]
    [BLLViewRole]
    [BLLSaveRole]
    [SampleSystemViewDomainObject(SampleSystemSecurityOperationCode.Disabled)]
    [SampleSystemEditDomainObject(SampleSystemSecurityOperationCode.Disabled)]
    public class MessageTemplateContainer : AuditPersistentDomainObjectBase
    {
        private SampleSystemMessageTemplate messageTemplate;

        public virtual SampleSystemMessageTemplate MessageTemplate
        {
            get => this.messageTemplate;
            set => this.messageTemplate = value;
        }
    }
}
