using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.IntergrationVersions;

namespace WorkflowSampleSystem.BLL
{
    public partial class IntegrationVersionContainer1BLL
    {
        public IntegrationVersionContainer1 IntegrationSave(IntegrationVersionContainer1CustomIntegrationSaveModel integrationSaveModel)
        {
            integrationSaveModel.SavingObject.Name = integrationSaveModel.CustomName;

            this.Save(integrationSaveModel.SavingObject);

            return integrationSaveModel.SavingObject;
        }
    }
}
