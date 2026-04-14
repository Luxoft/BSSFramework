using Framework.AutomationCore.RootServiceProviderContainer;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests.__Support.TestData.Helpers;

public partial class DataHelper(IServiceProvider rootServiceProvider) : RootServiceProviderContainer<ISampleSystemBLLContext>(rootServiceProvider)
{
    private Guid GetGuid(Guid? id)
    {
        id = id ?? Guid.NewGuid();
        return (Guid)id;
    }
}
