using Framework.DomainDriven.BLL;

using SampleSystem.Domain.IntergrationVersions;

namespace SampleSystem.Domain
{
    [BLLIntegrationSaveRole(AllowCreate = true)]
    public class IntegrationVersionContainer1CustomIntegrationSaveModel : DomainObjectIntegrationSaveModel<IntegrationVersionContainer1>
    {
        public string CustomName
        {
            get;
            set;
        }
    }
}
