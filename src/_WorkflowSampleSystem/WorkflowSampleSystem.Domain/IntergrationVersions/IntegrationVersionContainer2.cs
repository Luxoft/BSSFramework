using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace WorkflowSampleSystem.Domain.IntergrationVersions
{
    [DomainType("4FAB2A8D-600F-42E4-A72C-5B675BD60BAC")]
    [BLLIntegrationSaveRole]
    public class IntegrationVersionContainer2 : ExternalDomainObject
    {
        private string name;

        public virtual string Name
        {
            get => this.name;
            set => this.name = value;
        }

        [CustomSerialization(CustomSerializationMode.Ignore, DTORole.All ^ DTORole.Integration)]
        [IntegrationVersion(IntegrationPolicy = ApplyIntegrationPolicy.IgnoreLessVersion)]
        public override long IntegrationVersion
        {
            get => base.IntegrationVersion;
            set => base.IntegrationVersion = value;
        }
    }
}
