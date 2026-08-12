using SampleSystem.Domain.IntegrationVersions;
using SampleSystem.Domain.Models.Integration;

namespace SampleSystem.BLL;

public partial class IntegrationVersionContainer1BLL
{
    public IntegrationVersionContainer1 IntegrationSave(IntegrationVersionContainer1CustomIntegrationSaveModel integrationSaveModel)
    {
        integrationSaveModel.SavingObject.Name = integrationSaveModel.CustomName;

        this.Save(integrationSaveModel.SavingObject);

        return integrationSaveModel.SavingObject;
    }
}
