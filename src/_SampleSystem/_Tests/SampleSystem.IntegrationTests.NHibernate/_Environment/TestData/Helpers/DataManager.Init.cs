using Framework.AutomationCore.RootServiceProviderContainer;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests._Environment.TestData.Helpers;

public partial class DataManager(IServiceProvider rootServiceProvider) : IRootServiceProviderContainer<ISampleSystemBLLContext>
{
    private Guid GetGuid(Guid? id)
    {
        id = id ?? Guid.NewGuid();
        return (Guid)id;
    }

    public IServiceProvider RootServiceProvider { get; } = rootServiceProvider;
}
